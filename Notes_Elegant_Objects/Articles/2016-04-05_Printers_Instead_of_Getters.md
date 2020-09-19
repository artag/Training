# Printers Instead of Getters

[Link](https://www.yegor256.com/2016/04/05/printers-instead-of-getters.html)

Getters and setters
[are evil](https://www.yegor256.com/2014/09/16/getters-and-setters-are-evil.html).
No need to argue about this, it’s settled. You disagree? Let’s discuss that later.
For now, let’s say, we want to get rid of getters. The key question is how is it
possible at all? We do need to get the data out of an object, right? Nope. Wrong.

I’m suggesting to use "printers" instead. Instead of exposing data via getters, an
object will have a functionality of **printing** itself to some media.

Let’s say this is our class:

```java
public class Book {
  private final String isbn =
    "0735619654";
  private final String title =
    "Object Thinking";
}
```

We need it to be transferred into XML format. A more or less traditional way to do
it is via getters and
[JAXB](https://www.yegor256.com/2015/03/26/jaxb-vs-xembly.html):

```java
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
@XmlRootElement
public class Book {
  private final String isbn =
    "0735619654";
  private final String title =
    "Object Thinking";
  @XmlElement
  public String getIsbn() {
    return this.isbn;
  }
  @XmlElement
  public String getTitle() {
    return this.title;
  }
}
```

This is a very
[offensive](https://www.yegor256.com/2014/12/01/orm-offensive-anti-pattern.html)
way of treating the
[object](https://www.yegor256.com/2014/11/20/seven-virtues-of-good-object.html).
We’re basically exposing everything that’s inside to the public. It was a nice
little self-sufficient solid object and we turned it into a
[bag of data](https://www.yegor256.com/2016/07/06/data-transfer-object.html), which
anyone can access in many possible ways. We can access it for reading, of course.

It is convenient to have these getters, you may say. We are all used to them. If we want to convert it into
[JSON](https://www.yegor256.com/2015/11/16/json-vs-xml.html),
they will be very helpful. If we want to use this poor object as a data object in
JSP, getters will help us. There are many examples in Java, where getters are being
actively used.

This is **not** because they are so effective. This is because we’re so
[procedural](https://www.yegor256.com/2015/02/20/utility-classes-vs-functional-programming.html)
in our way of thinking. We **don’t trust** our objects. We only trust the data they
store. We don’t want this `Book` object to generate the XML. We want it to give us
the data. We will build the XML. The `Book` is too stupid to do that job. We’re way
smarter!

I’m suggesting to stop thinking this way. Instead, let’s try to give this poor
`Book` a chance, and equip it with a "printer":

```java
public class Book {
  private final String isbn =
    "0735619654";
  private final String title =
    "Object Thinking";
  public String toXML() {
    return String.format(
      "<book><isbn>%s</isbn><title>%s</title></book>",
      this.isbn, this.title
    );
  }
}
```

This isn’t the
[best](https://www.yegor256.com/2015/03/26/jaxb-vs-xembly.html) implementation,
but you got the idea. The object is not exposing its internals any more. We can’t
get its ISBN and its title. We can only ask it to print itself in XML format.

We can add an additional printer, if another format is required:

```java
public class Book {
  private final String isbn =
    "0735619654";
  private final String title =
    "Object Thinking";
  public String toJSON() {
    return String.format(
      "{\"isbn\":\"%s\", \"title\":\"%s\"}",
      this.isbn, this.title
    );
  }
}
```

Again, not the best implementation, but you see what I’m trying to show. Each time
we need a new format, we create a new printer.

You may say that the object will be rather big if there will be many formats.
That’s true, but a big object is a bad design in the first place. I would say that
if there is more than one printer - it’s a problem.

So, what to do if we need multiple formats? Use "media," where that printers will
be able to print to. Say, we have an object that represents a record in MySQL. We
want it to be printable to XML, HTML, JSON, some binary format and God knows what
else. We can add that many printers to it, but the object will be big and ugly. To
avoid that, introduce a new object, that represents the media where the data will
be printed to:

```java
public class Book {
  private final String isbn =
    "0735619654";
  private final String title =
    "Object Thinking";
  public Media print(Media media) {
    return media
      .with("isbn", this.isbn)
      .with("title", this.title);
  }
}
```

Again, it’s a very primitive design of that
[immutable](https://www.yegor256.com/2014/06/09/objects-should-be-immutable.html)
`Media` class, but you got the idea - the `media` accepts the data. Now, we want to
print our object to JSON (this design is not really perfect, since
`JsonObjectBuilder` is not immutable, even though it looks like one...):

```java
class JsonMedia implements Media {
  private final JsonObjectBuilder builder;
  JsonMedia() {
    this("book");
  }
  JsonMedia(String head) {
    this(Json.createObjectBuilder().add(head));
  }
  JsonMedia(JsonObjectBuilder bdr) {
    this.builder = bdr;
  }
  @Override
  public Media with(String name, String value) {
    return new JsonMedia(
      this.builder.add(name, value)
    );
  }
  public JsonObject json() {
    return this.builder.build();
  }
}
```

Now, we make an instance of `JsonMedia` and ask our book to print itself there:

```java
JsonMedia media = new JsonMedia("book");
media = book.print(media);
JsonObject json = media.json();
```

Voilà! The JSON object is ready and the book has no idea about what exactly what
printed just now. We need to print the book to XML? We create `XmlMedia`, which
will print the book to XML. The `Book` class stays small, while the complexity of
"media" objects is unlimited.

My point here is simple - no
[getters](https://www.yegor256.com/2014/09/16/getters-and-setters-are-evil.html),
just printers!
