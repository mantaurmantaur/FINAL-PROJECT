using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CoffeeShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerManager customerManager = new CustomerManager();
            Staff staff = new Staff();
            while (true)
            {
                // Display ASCII art and initial message
                string[] asciiArt = new string[]
                {
                    "             _                      ",
                    "     ,_ _   //  __   _,_ ,____,   _ ",
                    "_/_/_/_(/__(/__(_,__(_/_/ / / (__(/_"
                };

                foreach (string line in asciiArt)
                {
                    Console.WriteLine(line.PadLeft((Console.WindowWidth + line.Length) / 2));
                }

                Console.WriteLine("TO THE COFFEE SHOP".PadLeft((Console.WindowWidth + "TO THE COFFEE SHOP".Length) / 2));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Press Enter for CUSTOMERS...");
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // Hide the key input

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Customer Menu
                    while (true)
                    {
                        Console.Write("Are you a new customer?\n[0] YES\n[1] NO\n[2] EXIT\nEnter your choice: ");
                        int isNewCustomer;
                        bool validInput = int.TryParse(Console.ReadLine(), out isNewCustomer);

                        if (validInput && isNewCustomer == 0)
                        {
                            Console.Clear();
                            customerManager.RegisterNewCustomer();
                            break; // Return to the outer loop
                        }
                        else if (validInput && isNewCustomer == 1)
                        {
                            Console.Clear();
                            customerManager.HandleReturningCustomer();
                            break; // Return to the outer loop
                        }
                        else if (validInput && isNewCustomer == 2)
                        {
                            Console.Clear();
                            break; // Exit to the main menu
                        }
                        else
                        {
                            Console.WriteLine("Enter a valid choice, please.\n");
                            continue;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;

                    // Confirm if the user is staff
                    Console.WriteLine("It seems you pressed a non-customer key.");
                    Console.WriteLine("Are you staff?\n[0] YES\n[1] NO");
                    Console.ResetColor();

                    ConsoleKeyInfo staffConfirmKey = Console.ReadKey(intercept: true);
                    if (staffConfirmKey.Key == ConsoleKey.D0 || staffConfirmKey.Key == ConsoleKey.NumPad0)
                    {
                        // Proceed with staff login
                        Console.Clear();
                        const string correctPassword = "12345";
                        string enteredPassword = string.Empty;

                        while (enteredPassword.Length < 5)
                        {
                            ConsoleKeyInfo passwordKeyInfo = Console.ReadKey(intercept: true);

                            if (char.IsDigit(passwordKeyInfo.KeyChar))
                            {
                                enteredPassword += passwordKeyInfo.KeyChar;
                                Console.Write("*");
                            }
                            else if (passwordKeyInfo.Key == ConsoleKey.Backspace && enteredPassword.Length > 0)
                            {
                                enteredPassword = enteredPassword.Substring(0, enteredPassword.Length - 1);
                                Console.Write("\b \b");
                            }
                        }

                        Console.WriteLine();

                        if (enteredPassword == correctPassword)
                        {
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("What do you want to do?\n");
                                Console.WriteLine("[0] Add item in the Menu\n[1] Delete item in the Menu\n[2] Show Sales\n[3] Show Inventory\n[4] Update Inventory\n[5] Show Current Orders & Manage\n[6] Exit\n");
                                int staffChoice;
                                bool staffInputValid = int.TryParse(Console.ReadLine(), out staffChoice);

                                if (staffInputValid)
                                {
                                    int choices = staff.Operation(staffChoice);

                                    if (choices == 0)
                                    {
                                        continue; // Continue the inner loop
                                    }
                                    else if (choices == 1)
                                    {
                                        Console.WriteLine("Thank you for using the program.");
                                        Console.Clear();
                                        break; // Exit the staff menu loop and return to the outermost loop
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please try again.");
                                    Console.Clear();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Access Denied!");
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nReturning to the main menu...");
                        Console.Clear();
                    }
                }
            }


        }
    }

        public interface CoffeeMenuManagement
        {
            void AddCoffee();
            void DeleteCoffee();
        }

        public class Manager
        {
            public string filePathMenu = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\MenuCoffeee.txt";
            public string filePathHistory = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\OrderHistory.txt";
            public string filePathCustomers = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\CustomersOrders.txt";
            public string filePathInventory = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\Inventory.txt";
            public string filePathSalesReport = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\MonthlySales.txt";
            public string filePathCurrentOrders = "C:\\Users\\ann\\OneDrive\\Desktop\\skul stuffs\\2nd year\\Object Oriented Programming\\Final Project\\File Handling\\CurrentOrder.txt";
            public int changeOrder;

            public void AddCoffeeMenu()
            {
                List<string> newCoffees = new List<string>();//holds the added menu temporarily until confirmed by the staff
                List<string> newCoffeesinventory = new List<string>();//holds the added menu temporarily until confirmed by the staff
                try
                {

                    Console.WriteLine("\n\nHow many types of coffee are you gonna add?: ");
                    int addCoffenum = int.Parse(Console.ReadLine());

                    for (int i = 0; i < addCoffenum; i++)
                    {
                        Console.Write($"Enter the name of the coffee #{i+1} to be added: ");
                        string coffeename = Console.ReadLine();
                        Console.Write("Quantity: ");
                        int quantity = int.Parse(Console.ReadLine());
                        Console.Write($"Enter the price of the coffee #{i+1}: ");
                        int priceCoff = int.Parse(Console.ReadLine());

                        newCoffees.Add($"{coffeename},{priceCoff}");
                        newCoffeesinventory.Add($"{coffeename}, {quantity}");
                    }

                    Console.Write("\n\nDo you want to save the changes?: ");
                    string choice = Console.ReadLine();

                    if (choice == "yes" || choice == "y")
                    {
                        using (StreamWriter writer = File.AppendText(filePathMenu))
                        {
                            foreach (var coffee in newCoffees)
                            {
                                writer.WriteLine(coffee);
                            }
                        }

                        using (StreamWriter writer = File.AppendText(filePathInventory))
                        {
                            foreach (var coffee in newCoffeesinventory)
                            {
                                writer.WriteLine(coffee);
                            }
                        }
                        Console.WriteLine("Changes have been saved to the menu.");
                    }
                    else
                    {
                        Console.WriteLine("Changes have been discarded.");
                    }

                }
                catch (Exception ex)    
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void DelCoffeeMenu()
        {
            List<string> menuItems = File.ReadAllLines(filePathMenu).ToList();
            List<string> inventoryItems = File.ReadAllLines(filePathInventory).ToList(); // Assuming `filePathInventory` is the path to your inventory file

            // Lists to hold items to delete
            List<string> deleteCoffees = new List<string>();

            Console.Write("How many items are you going to delete? ");
            if (!int.TryParse(Console.ReadLine(), out int deleteChoice) || deleteChoice <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
                return;
            }

            // Loop to select items by number
            for (int i = 0; i < deleteChoice; i++)
            {
                Console.Write($"Enter the item number of the coffee to delete (1 to {menuItems.Count}): ");
                if (!int.TryParse(Console.ReadLine(), out int itemNumber) || itemNumber <= 0 || itemNumber > menuItems.Count)
                {
                    Console.WriteLine("Invalid item number. Please try again.");
                    i--; // Repeat this iteration for invalid input
                    continue;
                }

                itemNumber--; // Adjust for 0-based index
                deleteCoffees.Add(menuItems[itemNumber]); // Add item to delete list
            }

            // Confirm deletion
            Console.WriteLine("Do you want to save the changes? (yes/y to confirm)");
            string choice = Console.ReadLine()?.ToLower();

            if (choice == "yes" || choice == "y")
            {
                // Remove selected items from the menu and inventory
                foreach (var coffee in deleteCoffees)
                {
                    menuItems.Remove(coffee);
                    inventoryItems.RemoveAll(item => item.StartsWith(coffee.Split(',')[0].Trim())); // Match by item name
                }

                // Save the updated menu and inventory back to their respective files
                File.WriteAllLines(filePathMenu, menuItems);
                File.WriteAllLines(filePathInventory, inventoryItems);

                Console.WriteLine("Selected items have been deleted from the menu and inventory.");
            }
            else
            {
                Console.WriteLine("Changes have been discarded.");
            }
        }
            public void savetoSalesfile(int itemNumber, decimal quantity, string date)
        {
            List<string> menuItems = File.ReadAllLines(filePathMenu).ToList();

            // Retrieve item details from the menu
            var selectedItem = menuItems[itemNumber].Split(',');
            string itemName = selectedItem[0].Trim();
            decimal itemPrice = decimal.Parse(selectedItem[1].Trim());
            decimal subtotal = itemPrice * quantity;

            try
            {
                List<string> salesData = File.Exists(filePathSalesReport)
                    ? File.ReadAllLines(filePathSalesReport).ToList()
                    : new List<string>();

                bool dateFound = false;
                bool itemUpdated = false;

                List<string> updatedData = new();

                for (int i = 0; i < salesData.Count; i++)
                {
                    string line = salesData[i];

                    if (line.StartsWith("Date:") && line.Substring(5).Trim() == date)
                    {
                        dateFound = true;
                        updatedData.Add(line); // Add the date line
                        i++;

                        // Process items under this date
                        while (i < salesData.Count && !salesData[i].StartsWith("Date:"))
                        {
                            if (salesData[i].Trim() == "Item:")
                            {
                                updatedData.Add(salesData[i++]); // Add "Item:"
                                string existingItemName = salesData[i].Trim();

                                if (existingItemName == itemName)
                                {
                                    // Update existing item
                                    decimal existingQuantity = decimal.Parse(salesData[i + 1].Substring(9).Trim());
                                    decimal newQuantity = existingQuantity + quantity;

                                    updatedData.Add(existingItemName);
                                    updatedData.Add($"Quantity: {newQuantity}");
                                    updatedData.Add($"Price: {itemPrice}");

                                    i += 2; // Skip old quantity and price
                                    itemUpdated = true;
                                }
                                else
                                {
                                    // Add the existing item as is
                                    updatedData.Add(existingItemName);
                                    updatedData.Add(salesData[i + 1]);
                                    updatedData.Add(salesData[i + 2]);
                                    i += 2;
                                }
                            }
                            else
                            {
                                updatedData.Add(salesData[i]); // Add unrelated lines under the date
                            }

                            i++;
                        }

                        if (!itemUpdated)
                        {
                            // Add the new item if not found under this date
                            updatedData.Add("Item:");
                            updatedData.Add(itemName);
                            updatedData.Add($"Quantity: {quantity}");
                            updatedData.Add($"Price: {itemPrice}");
                        }

                        i--; // Adjust the index after processing items
                    }
                    else
                    {
                        updatedData.Add(line); // Add unrelated lines
                    }
                }

                if (!dateFound)
                {
                    // Add a new date entry if it doesn't exist
                    updatedData.Add($"Date: {date}");
                    updatedData.Add("Item:");
                    updatedData.Add(itemName);
                    updatedData.Add($"Quantity: {quantity}");
                    updatedData.Add($"Price: {itemPrice}");
                }

                // Write the updated data back to the file
                File.WriteAllLines(filePathSalesReport, updatedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the sales file: {ex.Message}");
            }
        }



            public void CalculateMonthlySales()
            {
                string itemName = string.Empty;
                int quantity = 0, totalquantity=0;
                decimal totalSales=0;


                while (true)
                {
                    Console.Write("[0] MONTHLY\n[1] WEEKLY\n[2] DAILY\nEnter your choice: ");
                    int type = int.Parse(Console.ReadLine());

                if (type == 0)
                {
                    int year;
                    int month;
                    while (true)
                    {
                        Console.Write("\nEnter the year (yyyy): ");
                        string yearInput = Console.ReadLine();
                        Console.Write("\nEnter the month (mm): ");
                        string monthInput = Console.ReadLine();

                        // Try to parse year and month into integers
                        if (int.TryParse(yearInput, out year) && yearInput.Length == 4 &&
                            int.TryParse(monthInput, out month) && month >= 1 && month <= 12)
                        {
                            // Valid input
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Please enter the year in 'yyyy' format and month in 'mm' format (1-12).");
                        }
                    }

                    try
                    {
                        if (!File.Exists(filePathSalesReport))
                        {
                            Console.WriteLine("No sales report file found.");
                            return;
                        }
                        string[] lines = File.ReadAllLines(filePathSalesReport);

                        Dictionary<string, (int TotalQuantity, decimal Subtotal)> itemSales = new();

                        DateTime currentOrderDate = DateTime.MinValue;
                        string currentItem = null;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith("Date:"))
                            {
                                string dateStr = lines[i].Substring(5).Trim();
                                currentOrderDate = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }

                            if (currentOrderDate.Year == year && currentOrderDate.Month == month)
                            {
                                if (lines[i].StartsWith("Item:"))
                                {
                                    currentItem = lines[i + 1].Trim(); // The item name is in the next line
                                }
                                else if (lines[i].StartsWith("Quantity:"))
                                {
                                    quantity = int.Parse(lines[i].Substring(9).Trim());
                                    decimal price = 0;

                                    // Look for the price in the following lines
                                    if (i + 1 < lines.Length && lines[i + 1].StartsWith("Price:"))
                                    {
                                        price = decimal.Parse(lines[i + 1].Substring(6).Trim());
                                    }

                                    if (currentItem != null)
                                    {
                                        // Update or add item sales data
                                        if (itemSales.ContainsKey(currentItem))
                                        {
                                            var existing = itemSales[currentItem];
                                            itemSales[currentItem] = (existing.TotalQuantity + quantity, existing.Subtotal + (quantity * price));
                                        }
                                        else
                                        {
                                            itemSales[currentItem] = (quantity, (quantity * price));
                                        }
                                    }
                                }
                            }
                        }

                        // Print the results
                        Console.WriteLine($"\nTotal Monthly Sales for {month}/{year}:");
                        foreach (var item in itemSales)
                        {
                            Console.WriteLine($"Item: {item.Key,-15} Quantity Sold: {item.Value.TotalQuantity,-15} Subtotal: PHP {item.Value.Subtotal,-15}");
                            totalSales += item.Value.Subtotal;
                            totalquantity += item.Value.TotalQuantity;

                        }

                        Console.WriteLine($"{new string(' ', 23)}Total Quantity: {totalquantity}{new string(' ', 10)}Total Sales: PHP {totalSales:F2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                    break;
                }
                else if (type == 1)
                {
                    int year, month, startDay, endDay;

                    while (true)
                    {
                        Console.Write("\nEnter the year (yyyy): ");
                        string yearInput = Console.ReadLine();
                        Console.Write("\nEnter the month (mm): ");
                        string monthInput = Console.ReadLine();
                        Console.Write("Enter the starting date (day): ");
                        string startDayInput = Console.ReadLine();
                        Console.Write("Enter the ending date (day): ");
                        string endDayInput = Console.ReadLine();

                        // Try to parse year and month into integers
                        if (int.TryParse(yearInput, out year) && yearInput.Length == 4 &&
                            int.TryParse(monthInput, out month) && month >= 1 && month <= 12
                            && int.TryParse(startDayInput, out startDay) && startDay >= 1 && startDay <= 31 &&
                            int.TryParse(endDayInput, out endDay) && endDay >= 1 && startDay <= 31)
                        {
                            // Valid input
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Please enter the year in 'yyyy' format, month in 'mm' format (1-12), and day (1-31).");
                        }
                    }

                    if (!File.Exists(filePathSalesReport))
                    {
                        Console.WriteLine("No order history file found.");
                        return;
                    }

                    try
                    {
                        string[] lines = File.ReadAllLines(filePathSalesReport);

                        // Dictionary to store item sales (key: item name, value: total quantity and subtotal)
                        Dictionary<string, (int TotalQuantity, decimal Subtotal)> itemSales = new();

                        DateTime currentOrderDate = DateTime.MinValue;
                        string currentItem = null;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith("Date:"))
                            {
                                string dateStr = lines[i].Substring(5).Trim();
                                currentOrderDate = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }

                            // Check if the current date is within the specified range
                            if (currentOrderDate.Year == year && currentOrderDate.Month == month && currentOrderDate.Day >= startDay && currentOrderDate.Day <= endDay)
                            {
                                if (lines[i].StartsWith("Item:"))
                                {
                                    // Read the item name from the next line
                                    currentItem = lines[i + 1].Trim();
                                }
                                else if (lines[i].StartsWith("Quantity:"))
                                {
                                    // Read quantity and price
                                    quantity = int.Parse(lines[i].Substring(9).Trim());
                                    decimal price = 0;

                                    if (i + 1 < lines.Length && lines[i + 1].StartsWith("Price:"))
                                    {
                                        price = decimal.Parse(lines[i + 1].Substring(6).Trim());
                                    }

                                    if (currentItem != null)
                                    {
                                        // Update or add item sales data
                                        if (itemSales.ContainsKey(currentItem))
                                        {
                                            var existing = itemSales[currentItem];
                                            itemSales[currentItem] = (existing.TotalQuantity + quantity, existing.Subtotal + (quantity * price));
                                        }
                                        else
                                        {
                                            itemSales[currentItem] = (quantity, quantity * price);
                                        }
                                    }
                                }
                            }
                        }

                        // Print the results
                        Console.WriteLine($"\nTotal Weekly Sales from {startDay}-{endDay}/{month}/{year}:");
                        foreach (var item in itemSales)
                        {
                            Console.WriteLine($"Item: {item.Key,-15} Quantity Sold: {item.Value.TotalQuantity,-15} Subtotal: PHP {item.Value.Subtotal,-15:F2}");
                            totalSales += item.Value.Subtotal;
                            totalquantity += item.Value.TotalQuantity;
                        }
                        Console.WriteLine($"{new string(' ', 23)}Total Quantity: {totalquantity}{new string(' ', 10)}Total Sales: PHP {totalSales:F2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                    break;
                }
                else if (type == 2)
                {

                    int year, month, Day;
                    string targetDate = "";

                    while (true)
                    {
                        Console.Write("\nEnter the year (yyyy): ");
                        string yearInput = Console.ReadLine();
                        Console.Write("\nEnter the month (mm): ");
                        string monthInput = Console.ReadLine();
                        Console.Write("\nEnter the date(day): ");
                        string DayInput = Console.ReadLine();



                        // Try to parse year and month into integers
                        if (int.TryParse(yearInput, out year) && yearInput.Length == 4 &&
                            int.TryParse(monthInput, out month) && month >= 1 && month <= 12
                            && int.TryParse(DayInput, out Day) && Day >= 1 && Day <= 31)
                        {
                            // Valid input
                            targetDate = $"{yearInput}-{monthInput}-{DayInput}";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Please enter the year in 'yyyy' format, month in 'mm' format (1-12), and day (1-31).");
                        }
                    }
                    try
                    {
                        if (!File.Exists(filePathSalesReport))
                        {
                            Console.WriteLine("Sales report file not found.");
                            return;
                        }

                        List<string> salesData = File.ReadAllLines(filePathSalesReport).ToList();
                        bool dateFound = false;
                        decimal totalQuantity=0;

                        Console.WriteLine($"\nTotal Sales for {Day}/{month}/{year}:");

                        for (int i = 0; i < salesData.Count; i++)
                        {
                            string line = salesData[i];

                            // Match the date
                            if (line.StartsWith("Date:") && line.Substring(5).Trim() == targetDate)
                            {
                                dateFound = true;
                                i++; // Move to the next line after "Date:"

                                // Process items under this date
                                while (i < salesData.Count && !salesData[i].StartsWith("Date:"))
                                {
                                    if (salesData[i].Trim() == "Item:")
                                    {
                                        // Extract item data
                                        itemName = salesData[i + 1].Trim();
                                        quantity = int.Parse(salesData[i + 2].Substring(9).Trim());
                                        decimal price = decimal.Parse(salesData[i + 3].Substring(6).Trim());

                                        decimal subtotal = quantity * price;
                                        totalQuantity += quantity;
                                        totalSales += subtotal;

                                        Console.WriteLine($"Item: {itemName,-20} Quantity Sold: {quantity,-10} Subtotal: PHP {subtotal:F2}");
                                        i += 3; // Skip to the next potential "Item:" or "Date:"
                                    }
                                    else
                                    {
                                        i++; // Skip unrelated lines
                                    }
                                }

                                i--; // Adjust to avoid skipping the next "Date:" line
                            }
                        }

                        if (!dateFound)
                        {
                            Console.WriteLine($"No sales data found for {Day}/{month}/{year}.");
                        }
                        else
                        {
                            Console.WriteLine($"\n{"Total Quantity:",-30} {totalQuantity}");
                            Console.WriteLine($"{"Total Sales:",-30} PHP {totalSales:F2}");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }


                else
                {
                    Console.WriteLine("Please enter a valid choice.");
                    continue;
                }
                }
            }

            public void OrderHistory(string CustomerName)
            {
                try
                {
                    if (File.Exists(filePathHistory))
                    {
                        string[] lines = File.ReadAllLines(filePathHistory);
                        bool foundOrders = false;//nganong false man?
                        Console.WriteLine($"\nOrder History for {CustomerName}:\n");

                        for (int i = 0; i < lines.Length; i++)
                        {
                            // Check if the current line is for the specified customer
                            if (lines[i].StartsWith("Customer:") && lines[i].Contains(CustomerName))
                            {
                                foundOrders = true;

                                // Print the order details for this customer
                                while (i < lines.Length && !lines[i].StartsWith("---END ORDER---"))
                                {
                                    Console.WriteLine(lines[i]);
                                    i++;
                                }
                                Console.WriteLine("---END ORDER---\n");
                            }
                        }

                        if (!foundOrders)
                        {
                            Console.WriteLine("No orders found for this customer.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Order history file not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while reading order history: " + ex.Message);
                }
            }


            public Dictionary<string, int> LoadInventory()
            {
                var inventory = new Dictionary<string, int>();

                try
                {
                    if (File.Exists(filePathInventory))
                    {
                        foreach (var line in File.ReadLines(filePathInventory))
                        {
                            var parts = line.Split(',');
                            if (parts.Length < 2)
                            {
                                Console.WriteLine($"Invalid inventory entry: {line}");
                                continue; // Skip invalid lines
                            }

                            string productName = parts[0].Trim(); // Trim to remove leading/trailing spaces
                            if (int.TryParse(parts[1], out int quantity))
                            {
                                inventory[productName] = quantity;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid quantity for product '{productName}': {parts[1]}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Inventory file not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading inventory: " + ex.Message);
                }

                return inventory;
            }


            public void UpdateInventory(Dictionary<string, int> inventory)//for orders only
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePathInventory))
                    {
                        foreach (var item in inventory)
                        {
                            writer.WriteLine($"{item.Key}, {item.Value}");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error updating inventory.");
                }
            }

            public void UpdateinventoryManual(string productName, int quantity)
            {
                try
                {
                    var inventory = LoadInventory();
                    {
                        if (inventory.ContainsKey(productName))
                        {
                            inventory[productName] += quantity;
                        }
                        else
                        {
                            inventory[productName] = quantity;  // If the product doesn't exist, add it.
                        }
                    }
                    UpdateInventory(inventory);
                }
                catch
                {
                    Console.WriteLine("Error updating inventory.");
                }
            }

            public int ProcessOrder(string productName, int quantity)
            {

                var inventory = LoadInventory();

                if (inventory.ContainsKey(productName) && inventory[productName] >= quantity)
                {
                    inventory[productName] -= quantity;
                    Console.WriteLine($"Order Processed: {quantity} {productName}(s) sold.");
                    UpdateInventory(inventory);
                    return 0;
                }
                else if (inventory.ContainsKey(productName) && inventory[productName] < quantity)
                {
                    Console.WriteLine($"Insufficient stock for {productName}. Available quantity: {inventory[productName]}.");
                    Console.WriteLine("We are really sorry for the inconvenice. Please change your order. ");
                    return 1;//katulo siya ni print, find the problem in here
                }
                else
                {
                    Console.WriteLine($"{productName} is not available in inventory.");
                    return 2;
                }
            }


            public void ShowMenu()
            {
                try
                {
                    if (File.Exists(filePathMenu))
                    {
                        string[] lines = File.ReadAllLines(filePathMenu);
                        Console.WriteLine("Coffee Menu".PadLeft((Console.WindowWidth + "Coffee Menu".Length) / 2));
                        Console.WriteLine("-----------------------------------------".PadLeft((Console.WindowWidth + "-----------------------------------------".Length) / 2));

                        for (int i = 0; i < lines.Length; i++)
                        {
                            // Ensure the line is properly formatted with a comma
                            if (lines[i].Contains(","))
                            {
                                var item = lines[i].Split(',');
                                string coffeeName = item[0].Trim();
                                string price = item[1].Trim();

                                // Display item number
                                if (i + 1 < 10)
                                {
                                    Console.WriteLine($"[0{i + 1}] {coffeeName,-25} PHP {price}".PadLeft((Console.WindowWidth + $"[0{i + 1}] {coffeeName,-25} PHP {price}".Length) / 2));
                                }
                                else
                                {
                                    Console.WriteLine($"[{i + 1}] {coffeeName,-25} PHP {price}".PadLeft((Console.WindowWidth + $"[{i + 1}] {coffeeName,-25} PHP {price}".Length) / 2));
                                }

                            }
                            else
                            {
                                Console.WriteLine("Invalid format in menu file.");
                            }
                        }

                        Console.WriteLine("-----------------------------------------".PadLeft((Console.WindowWidth + "-----------------------------------------".Length) / 2));
                    }
                    else
                    {
                        Console.WriteLine("Menu file not found. Please add items to the menu.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while loading the menu: " + ex.Message);
                }
            }

            public void ShowInventory()
            {
                try
                {
                    if (File.Exists(filePathInventory))
                    {
                        string[] lines = File.ReadAllLines(filePathInventory);
                        Console.WriteLine("\nInventory:");
                        Console.WriteLine("-----------------------------------------");

                        for (int i = 0; i < lines.Length; i++)
                        {
                            // Ensure the line is properly formatted with a comma
                            if (lines[i].Contains(","))
                            {
                                var item = lines[i].Split(',');
                                string coffeeName = item[0].Trim();
                                string quantity = item[1].Trim();

                                // Display item number
                                if (i + 1 < 10)
                                {
                                    Console.WriteLine($"[0{i + 1}] {coffeeName,-25} pcs {quantity}");
                                }
                                else
                                {
                                    Console.WriteLine($"[{i + 1}] {coffeeName,-25} pcs {quantity}");
                                }

                            }
                            else
                            {
                                Console.WriteLine("Invalid format in menu file.");
                            }
                        }

                        Console.WriteLine("-----------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Menu file not found. Please add items to the menu.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while loading the menu: " + ex.Message);
                }
            }

        public void Payment(decimal totalAmount, List<string> allOrderDetails)
            {
                Console.WriteLine("\n\n\n\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("PAYMENT".PadLeft((Console.WindowWidth + "PAYMENT".Length) / 2));
                Console.ResetColor();
                
                while (true)
                {
                    Console.Write("Please enter the amount: ");
                    decimal Amount = decimal.Parse(Console.ReadLine());

                    if (Amount < totalAmount)
                    {
                        Console.WriteLine("Not enough, add more.");
                        continue;
                    }
                    else
                    {
                        decimal change = Amount - totalAmount;

                    Console.WriteLine("\nPayment successful!\nThank you for ordering, come again!");
                    Console.WriteLine("\n\n\nReceipt:");
                    Console.WriteLine("Orders:");
                    foreach (string order in allOrderDetails)
                    {
                        string[] details = order.Split(',');
                        string itemName = details[0];
                        int quantity = int.Parse(details[1]);

                        Console.WriteLine($"{itemName} {quantity}");
                    }
                    Console.WriteLine($"Total Amount: {totalAmount}");
                    Console.WriteLine($"Given Amount: {Amount}");
                    Console.WriteLine($"Change: {change}");
                    break;
                    }

                }
                

        }

            public void ShowCurrentOrders()//for staff
            {
                try
                {
                    // Step 1: Read all lines from the file
                    var lines = File.ReadAllLines(filePathCurrentOrders).ToList();

                    
                    if (lines.Count == 0 || !lines.Any(line => line.StartsWith("Customer:")))
                    {
                        Console.WriteLine("All orders are received. No pending orders.");
                        return;
                    }
                    // Step 2: Display all orders
                    Console.WriteLine("\nCurrent Orders:\n");
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }

                Console.WriteLine("\nChoose what to do:\n[1]Do you want to delete an order (order already received)?\n[2] Go back\n");
                Console.WriteLine("Enter choice: ");
                int choice = int.Parse(Console.ReadLine());
                if(choice == 1)
                {
                    // Step 3: Ask for customer name to remove
                    Console.Write("\nEnter the customer's name whose order you want to remove: ");
                    string customerName = Console.ReadLine();

                    // Step 4: Initialize variables to track orders
                    List<string> updatedLines = new();
                    bool isMatchingOrder = false;

                    // Step 5: Loop through each line and find the matching order
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("Customer:"))
                        {
                            // Check if this is the matching customer
                            isMatchingOrder = line.Contains(customerName);
                        }

                        // Add lines to the updated list if it's not the matching order
                        if (!isMatchingOrder)
                        {
                            updatedLines.Add(line);
                        }

                        // If we reach "---END ORDER---", reset the matching flag
                        if (line.StartsWith("---END ORDER---"))
                        {
                            isMatchingOrder = false;
                        }
                    }

                    // Step 6: Confirm and rewrite the file
                    Console.Write($"\nAre you sure you want to remove the order for '{customerName}'? (yes/no): ");
                    string confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation == "yes" || confirmation == "y")
                    {
                        File.WriteAllLines(filePathCurrentOrders, updatedLines);
                        Console.WriteLine($"Order for customer '{customerName}' has been removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No orders were removed.");
                    }
                }
                    
                else
                {
                    return;
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

        public void savetoCurrentOrders(decimal totalAmount, string customerName, List<string> allOrderDetails, string notes)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePathCurrentOrders, true))
                {
                    writer.WriteLine($"Customer: {customerName}");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine("Items:");

                    foreach (var detail in allOrderDetails)
                    {
                        var itemParts = detail.Split(',');
                        string itemName = itemParts[0];
                        int quantity = int.Parse(itemParts[1]);
                        decimal price = decimal.Parse(itemParts[2]);
                        decimal subtotal = decimal.Parse(itemParts[3]);
                        string sugarLevel = itemParts[4];

                        writer.WriteLine($"{itemName} - Quantity: {quantity}, Price: PHP {price}, Subtotal: PHP {subtotal}");
                        writer.WriteLine($"    Sugar Level: {sugarLevel}");
                    }

                    writer.WriteLine($"Total Amount: PHP {totalAmount}");
                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        writer.WriteLine($"Notes: {notes}");
                    }
                    writer.WriteLine("---END ORDER---");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the order: " + ex.Message);
            }
        }


        }

        public class Staff : Manager, CoffeeMenuManagement
        {
            public int Operation(int choice)
            {
                int choice1;
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        ShowMenu();
                        AddCoffee();
                        Console.Clear();
                        ShowMenu();
                        break;
                    case 1:
                        ShowMenu();
                        DeleteCoffee();
                        ShowMenu();
                        break;
                    case 2:
                        CalculateMonthlySales();
                        break;
                    case 3:
                        ShowInventory();
                        break;
                    case 4:
                        ShowInventory();
                        Console.Write("Enter the name of the coffee: ");
                        string coffeeName = Console.ReadLine();
                        Console.Write($"Enter the quantity of the {coffeeName} to add: ");
                        int quantity = int.Parse(Console.ReadLine());

                        UpdateinventoryManual(coffeeName, quantity);
                        break;
                    case 5:
                        ShowCurrentOrders();
                        break;
                    case 6:
                        return 1;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 0 and 3.");
                        break;
                }

                while (true)
                {
                    // Prompt to continue after each operation
                    Console.Write("\n\nEnter 0(zero) to continue: ");
                        choice1 = int.Parse(Console.ReadLine());
                    
                    if (choice1 != 0)
                    {
                        continue;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }

            public void AddCoffee()
            {
                AddCoffeeMenu();
            }

            public void DeleteCoffee()
            {
                DelCoffeeMenu();
            }

        }

        public class CustomerManager : Manager
        {
            public string CustomerName;
            public List<string> orderItems = new List<string>();
            public List<string> orderDetails = new List<string>();
            public decimal totalAmount;
            public int choice;

            public void RegisterNewCustomer()
            {
                Console.Write("Enter Customer Name: ");
                string CustomerName = Console.ReadLine();

                while (true)
                {
                    bool customerExists = false;

                    try
                    {
                        if (File.Exists(filePathCustomers))
                        {
                            string[] existingCustomers = File.ReadAllLines(filePathCustomers);
                            customerExists = Array.Exists(existingCustomers, line => line.Trim() == CustomerName.Trim());
                        }

                        if (customerExists)
                        {
                            Console.WriteLine("\nCustomer already registered.Please enter the same name if you are already a customer here and change the name if it is your first time in here.\n");
                            HandleReturningCustomer(); // Redirect to handle returning customer
                            break;
                        }
                        else
                        {
                            // Register new customer
                            using (StreamWriter writer = new StreamWriter(filePathCustomers, true))
                            {
                                writer.WriteLine(CustomerName);
                                Console.WriteLine("Customer registered successfully.");
                            }

                            // Display menu and handle order
                            ShowMenu();
                            Console.WriteLine("Place an Order:".PadLeft((Console.WindowWidth + "Place an Order:".Length) / 2));
                            PlaceOrder(CustomerName); // Place the order for the new customer
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while registering the customer: " + ex.Message);
                    }

                    // Prompt to continue or end the program
                    Console.Write("\n\nDo you have any other orders or want to change the quantity of your orders? (yes/y to continue): ");
                    string choiceEnd = Console.ReadLine()?.ToLower();//to lower converts the answer of the customer from uppercase to lowercase

                    if (choiceEnd == "yes" || choiceEnd == "y")
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("End of the program. Thank you for using!");
                        Console.Clear();
                        break;
                    }
                }
            }

            public void HandleReturningCustomer()
            {
                Console.Write("Enter Customer Name: ");
                string customerName = Console.ReadLine();

                while (true)
                {
                    bool customerExists = false;

                    try
                    {
                        if (File.Exists(filePathCustomers))
                        {
                            string[] existingCustomers = File.ReadAllLines(filePathCustomers);
                            customerExists = Array.Exists(existingCustomers, line => line.Trim().Equals(customerName.Trim(), StringComparison.OrdinalIgnoreCase));
                        }

                        if (customerExists)
                        {
                            int returningCustomerChoice;

                            do
                            {
                                Console.WriteLine("\nYou are already registered. What do you want to do:\n[0] View Order History\n[1] Place an order\n[2] Exit\n");
                                Console.Write("Enter your choice: ");
                                returningCustomerChoice = int.Parse(Console.ReadLine());

                                if (returningCustomerChoice == 0)
                                {
                                    OrderHistory(customerName);
                                    break;
                                }
                                else if (returningCustomerChoice == 1)
                                {
                                    ShowMenu();
                                    PlaceOrder(customerName);
                                    break;
                                }
                                else if (returningCustomerChoice == 2)
                                {
                                    Console.Clear();
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Choose only 0 or 1");
                                }
                            } while (returningCustomerChoice != 0 && returningCustomerChoice != 1);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\nYou are not registered, please register first to order.");
                            RegisterNewCustomer();
                            break; // i exit ang loop inig human ug register
                        }

                        Console.Write("\n\nDo you have any other orders? Or do you want to change the quantity of your orders? (yes/y to continue): ");//change this
                        string choiceEnd = Console.ReadLine()?.ToLower();

                        if (choiceEnd == "yes" || choiceEnd == "y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("End of the program. Thank you for using!");
                            Console.Clear();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

           public void PlaceOrder(string customerName)
            {
                decimal totalAmount = 0;
                List<string> allOrderDetails = new List<string>();

                Console.WriteLine($"\nWelcome, {customerName}! Let's start your order.\n");

                Console.Write("How many types of coffee would you like to order?: ");
                if (!int.TryParse(Console.ReadLine(), out int orderCount) || orderCount <= 0)
                {
                    Console.WriteLine("Invalid order count. Please enter a positive integer.");
                    return;
                }

                List<string> menuItems = File.ReadAllLines(filePathMenu).ToList();

                for (int i = 0; i < orderCount; i++)
                {
                    Console.Write($"\nEnter the item number {i + 1}: ");
                    if (!int.TryParse(Console.ReadLine(), out int itemNumber) || itemNumber <= 0 || itemNumber > menuItems.Count)
                    {
                        Console.WriteLine("Invalid item number. Please try again.");
                        i--; // Retry this iteration
                        continue;
                    }

                    itemNumber--; // Adjust for 0-based index

                    var selectedItem = menuItems[itemNumber].Split(',');
                    string itemName = selectedItem[0].Trim();
                    decimal itemPrice = decimal.Parse(selectedItem[1].Trim());

                    Console.Write($"Enter quantity for {itemName}: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity. Please enter a positive integer.");
                        i--;
                        continue;
                    }

                    string sugarLevelText = "None";
                    while (true)
                    {
                        Console.WriteLine("Choose sugar level (for free):");
                        Console.WriteLine("[0] 100%\n[1] 80%\n[2] 60%\n[3] 50%\n[4] 30%\n[5] 10%\n[6] None");

                        if (int.TryParse(Console.ReadLine(), out int notesSugar))
                        {
                            switch (notesSugar)
                            {
                                case 0: sugarLevelText = "100%"; break;
                                case 1: sugarLevelText = "80%"; break;
                                case 2: sugarLevelText = "60%"; break;
                                case 3: sugarLevelText = "50%"; break;
                                case 4: sugarLevelText = "30%"; break;
                                case 5: sugarLevelText = "10%"; break;
                                case 6: sugarLevelText = "None"; break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    continue;
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number between 0 and 6.");
                        }
                    }

                    decimal priceAdd = 0;

                    decimal subtotal = (itemPrice * quantity) + priceAdd;
                    totalAmount += subtotal;

                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    savetoSalesfile(itemNumber, quantity, date);
                    
                    allOrderDetails.Add($"{itemName},{quantity},{itemPrice},{subtotal},{sugarLevelText}");
                }

                // Process and display all orders at the end
                foreach (string order in allOrderDetails)
                {
                    string[] details = order.Split(',');
                    string itemName = details[0];
                    int quantity = int.Parse(details[1]);

                    Console.WriteLine();
                    int processResult = ProcessOrder(itemName, quantity);
                    if (processResult != 0)
                    {
                        Console.WriteLine($"\nUnable to process {itemName}. Order cancelled for this item.");
                        return; // Skip to the next item if processing failed
                    }
                }

                string notes = Notes();
                savetoOrderHistory(totalAmount, customerName, allOrderDetails, notes);
                savetoCurrentOrders(totalAmount, customerName, allOrderDetails, notes);
                Console.Clear();
                PrintOrder(customerName, totalAmount, allOrderDetails, notes);
                Payment(totalAmount, allOrderDetails );
                
            }

            public void savetoOrderHistory(decimal totalAmount, string customerName, List<string> allOrderDetails, string notes)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePathHistory, true))
                    {
                        writer.WriteLine($"Customer: {customerName}");
                        writer.WriteLine($"Date: {DateTime.Now}");
                        writer.WriteLine("Items:");

                        foreach (var detail in allOrderDetails)
                        {
                            var itemParts = detail.Split(',');
                            string itemName = itemParts[0];
                            int quantity = int.Parse(itemParts[1]);
                            decimal price = decimal.Parse(itemParts[2]);
                            decimal subtotal = decimal.Parse(itemParts[3]);
                            string sugarLevel = itemParts[4];

                            writer.WriteLine($"{itemName} - Quantity: {quantity}, Price: PHP {price}, Subtotal: PHP {subtotal}");
                            writer.WriteLine($"    Sugar Level: {sugarLevel}");
                        }

                        writer.WriteLine($"Total Amount: PHP {totalAmount}");
                        if (!string.IsNullOrWhiteSpace(notes))
                        {
                            writer.WriteLine($"Notes: {notes}");
                        }
                        writer.WriteLine("---END ORDER---");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the order: " + ex.Message);
                }
            }

            public string Date()
                {
                    string orderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    return orderDate;
                }

            public void PrintOrder(string customerName, decimal totalAmount, List<string> allOrderDetails, string notes)
            {

                Console.WriteLine();
                Console.WriteLine("---Current Order Summary---".PadLeft((Console.WindowWidth + "---Current Order Summary---".Length) / 2));
                Console.WriteLine($"Customer: {customerName}".PadLeft((Console.WindowWidth + $"Customer: {customerName}".Length) / 2));
                Console.WriteLine($"Date: {DateTime.Now}".PadLeft((Console.WindowWidth + $"Date: {DateTime.Now}".Length) / 2));
                Console.WriteLine("Items:".PadLeft((Console.WindowWidth + "Items:".Length) / 2));

                foreach (var detail in allOrderDetails)
                {
                    var itemParts = detail.Split(',');
                    string itemName = itemParts[0];
                    int quantity = int.Parse(itemParts[1]);
                    decimal price = decimal.Parse(itemParts[2]);
                    decimal subtotal = decimal.Parse(itemParts[3]);
                    string sugarLevel = itemParts[4];

                    Console.WriteLine($"{itemName} - Quantity: {quantity}, Price: PHP {price}, Subtotal: PHP {subtotal}".PadLeft((Console.WindowWidth + $"{itemName} - Quantity: {quantity}, Price: PHP {price}, Subtotal: PHP {subtotal}".Length) / 2));
                    Console.WriteLine($"Sugar Level: {sugarLevel}".PadLeft((Console.WindowWidth + $"Sugar Level: {sugarLevel}".Length) / 2));
                }

                Console.WriteLine($"Total Amount: PHP {totalAmount}".PadLeft((Console.WindowWidth + $"Total Amount: PHP {totalAmount}".Length) / 2));
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    Console.WriteLine($"Notes: {notes}".PadLeft((Console.WindowWidth + $"Notes: {notes}".Length) / 2));
                }
                Console.WriteLine("-----------------------------------------".PadLeft((Console.WindowWidth + "-----------------------------------------".Length) / 2));

            }

            public string Notes()
                {
                    Console.Write("Notes and Allergies for this item: ");
                    string notes = Console.ReadLine();
                    return notes;
                } 
            }
}




