namespace Nulls
{
    public class Controller
    {
        public ActionResult HttpNotFound()
        {
            return null;
        }

        public ActionResult RedirectToAction(string name)
        {
            return null;
        }

        public ActionResult View()
        {
            return null;
        }

        public ActionResult View(Customer customer)
        {
            return null;
        }

        public ActionResult View(string name)
        {
            return null;
        }

        public ActionResult View(string name, string message)
        {
            return null;
        }

        public ModelState ModelState { get; set; }
    }
}
