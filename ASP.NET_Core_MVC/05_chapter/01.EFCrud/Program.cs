using System;
using System.Linq;
using System.Threading.Tasks;
using EFCrud.Entities;
using static System.ConsoleKey;

namespace EFCrud;

public class Program
{
    static async Task Main(string[] args)
    {
        var key = new ConsoleKeyInfo();
        while (key.Key != D0)
        {
            Console.Clear();
            Console.WriteLine("Select operation:");
            DisplayLine();
            Console.WriteLine("1. Add a mobile device.");
            Console.WriteLine("2. List all mobile devices.");
            Console.WriteLine("3. Display the mobile device.");
            Console.WriteLine("4. Edit the mobile device.");
            Console.WriteLine("5. Delete the mobile device.");
            Console.WriteLine("0. Quit.");

            key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case D1:
                    await AddMobileDevice();
                    break;
                case D2:
                    await ListAllMobileDevices();
                    break;
                case D3:
                    await DisplayMobileDevice();
                    break;
                case D4:
                    await EditMobileDevice();
                    break;
                case D5:
                    await DeleteMobileDevice();
                    break;
                case D0:
                    break;
            }
        }

        Console.WriteLine("Press any key to quit...");
        Console.ReadKey();
    }

    private static async Task AddMobileDevice()
    {
        Console.Clear();
        Console.WriteLine("Operation: Add a new mobile device.");
        DisplayLine();

        Console.Write("Enter model: ");
        var model = Console.ReadLine();
        Console.Write("Enter phone number: ");
        var phone = Console.ReadLine();

        var device = new MobileDevice { Model = model ?? string.Empty, ClientPhone = phone ?? string.Empty };
        var id = await Operations.Create(device);
        if (id < 1)
            Console.WriteLine("Error. New mobile device not created");
        else
            Console.WriteLine("Created new mobile device.");

        WaitKeyPressing();
    }

    private static async Task ListAllMobileDevices()
    {
        Console.Clear();
        Console.WriteLine("Operation: List all mobile devices.");
        DisplayLine();

        var devices = await Operations.Read();
        if (!devices.Any())
        {
            Console.WriteLine("No mobile devices were found.");
            WaitKeyPressing();
            return;
        }

        foreach (var device in devices)
        {
            DisplayMobileDevice(device);
        }

        WaitKeyPressing();
    }

    private static async Task DisplayMobileDevice()
    {
        Console.Clear();
        Console.WriteLine("Operation: Display the mobile device.");
        DisplayLine();

        Console.Write("Enter mobile device id: ");
        var idString = Console.ReadLine();
        var id = int.Parse(idString ?? "0");

        var device = await Operations.Read(id);
        if (device == null)
        {
            Console.WriteLine($"No mobile device with id = {id} was found.");
            WaitKeyPressing();
            return;
        }

        DisplayMobileDevice(device!);
        WaitKeyPressing();
    }

    private static async Task EditMobileDevice()
    {
        Console.Clear();
        Console.WriteLine("Operation: Edit the mobile device.");
        DisplayLine();

        Console.Write("Enter mobile device id: ");
        var idString = Console.ReadLine();
        var id = int.Parse(idString ?? "0");

        var device = await Operations.Read(id);
        if (device == null)
        {
            Console.WriteLine($"No mobile device with id = {id} was found.");
            WaitKeyPressing();
            return;
        }

        DisplayMobileDevice(device!);
        Console.WriteLine();

        Console.Write("Enter new model: ");
        var model = Console.ReadLine();
        Console.Write("Enter new phone number: ");
        var phone = Console.ReadLine();

        var editedDevice = new MobileDevice
        {
            Id = device.Id,
            Model = model ?? string.Empty,
            ClientPhone = phone ?? string.Empty
        };

        await Operations.Update(editedDevice);

        Console.WriteLine("Mobile device was updated.");
        WaitKeyPressing();
    }

    private static async Task DeleteMobileDevice()
    {
        Console.Clear();
        Console.WriteLine("Operation: Delete the mobile device.");
        DisplayLine();

        Console.Write("Enter mobile device id: ");
        var idString = Console.ReadLine();
        var id = int.Parse(idString ?? "0");

        var device = await Operations.Read(id);
        if (device == null)
        {
            Console.WriteLine($"No mobile device with id = {id} was found.");
            WaitKeyPressing();
            return;
        }

        await Operations.Delete(device);

        Console.WriteLine($"Mobile device with id = {id} was deleted.");
        WaitKeyPressing();
    }

    private static void DisplayMobileDevice(MobileDevice device)
    {
        Console.WriteLine($"Id: {device.Id}");
        Console.WriteLine($"Model: {device.Model}");
        Console.WriteLine($"Phone number: {device.ClientPhone}");
        DisplayLine();
    }

    private static void WaitKeyPressing()
    {
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private static void DisplayLine()
    {
        Console.WriteLine(new string('-', 40));
    }
}
