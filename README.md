### BellyRedNovel
### Car Inventory Management System
This is a simple WPF (Windows Presentation Foundation) application written in C# for managing a car inventory. The application allows users to add, sort, and save a list of cars.

Features:
Sorting:

Users can sort the list of cars by various criteria such as Make, Model, Year, Price, and Mileage.
Clicking a sorting button toggles between ascending and descending order.
Adding Vehicles:

Users can input details of a new vehicle (Make, Model, Year, Price, Mileage) and add it to the inventory.
The new vehicle is inserted at the correct position based on the sorting criteria.
Saving to CSV:

Users can save the current list of cars to a CSV file.
The system prompts users to enter a file name, and the list is saved with that name in CSV format.
Preloaded Data:

The system comes with preloaded data for demonstration purposes.
Users can view, sort, and add to the existing list of cars.
Code Structure:
MainWindow.xaml:
Defines the layout of the main window with buttons, labels, textboxes, and a ListView for displaying cars.
Provides sorting options and input fields for adding new vehicles.
MainWindow.xaml.cs:
Defines the MainWindow class, which includes event handlers for button clicks and various methods for sorting, adding, and saving.
Implements sorting functionality using Merge Sort on an ObservableCollection of Car objects.
Manages the overall flow of the application.
Car.cs:
Defines the Car class, a simple data model with properties like Make, Model, Year, Price, and Mileage.
Includes constructors for initializing instances and an overridden ToString method for displaying car information.
How to Use:
Sorting:

Click on the sorting buttons (Make, Model, Year, Price, Mileage) to sort the list of cars.
Adding Vehicles:

Enter details in the input fields (Make, Model, Year, Price, Mileage).
Click the "Add Vehicle" button to add the new vehicle to the list.
Saving to CSV:

Enter a desired file name in the "File Name" textbox.
Click the "Save" button to save the list of cars as a CSV file.
Preloaded Data:

The application starts with preloaded data for demonstration purposes.
Users can explore, sort, and add to the existing list of cars.
