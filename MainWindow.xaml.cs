using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Transactions;
using System.Windows;

namespace BellyRedNovel
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Car> cars = new ObservableCollection<Car>(); // list for all the cars

        public MainWindow()
        {
            InitializeComponent();
            UpdateListView();
            PreloadCars();       // preload cars and update cars list view       
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
          
            // Retrieve data from textboxes
            string make = MakeTB.Text;
            string model = ModelTB.Text;

            // Check if Year is a valid number
            if (!int.TryParse(YearTB.Text, out int year))
            {
                MessageBox.Show("Please enter a valid number for the Year field.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if Price is a valid number
            if (!decimal.TryParse(PriceTB.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid number for the Price field.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if Mileage is a valid number
            if (!int.TryParse(MileageTB.Text, out int mileage))
            {
                MessageBox.Show("Please enter a valid number for the Mileage field.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // make sure all fields are valid before adding 
            if (string.IsNullOrEmpty(MakeTB.Text) || string.IsNullOrEmpty(ModelTB.Text) ||
            string.IsNullOrEmpty(YearTB.Text) || string.IsNullOrEmpty(PriceTB.Text) || string.IsNullOrEmpty(MileageTB.Text))
            {
                MessageBox.Show("Please enter a valid responses.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }



            // Create a new Car instance
            Car newCar = new Car()
            {
                Make = make,
                Model = model,
                Year = year,
                Price = price,
                Mileage = mileage
            };

            // Find the index where the new car should be inserted based on the sorting criteria 
            int index = FindInsertIndex(newCar, car => car.Make);

            // Insert the new car at the correct position in the ObservableCollection (Not really a feature yet but is there If I want to go deaper on project)
            cars.Insert(index, newCar);

            //  clear the textboxes after adding a car
            MakeTB.Clear();
            ModelTB.Clear();
            YearTB.Clear();
            PriceTB.Clear();
            MileageTB.Clear();

            CarLV.Items.Refresh();
            
        }

        private int FindInsertIndex<T>(Car newCar, Func<Car, T> keySelector) where T : IComparable<T>
        {
            int index = 0;

            // Find the index where the new car should be inserted based on the sorting criteria
            while (index < cars.Count && keySelector(newCar).CompareTo(keySelector(cars[index])) >= 0)
            {
                index++;
            }

            return index;
        }

        public void UpdateListView() // refresh the cars list view
        {
            CarLV.ItemsSource = cars;
            CarLV.Items.Refresh();
        }

        private void MergeSortCars<T>(List<Car> cars, int left, int right, Func<Car, T> keySelector) where T : IComparable<T>
        {
            // Check if there is more than one element in the array
            if (left < right)
            {
                // Calculate the middle index
                int middle = (left + right) / 2;

                // Recursive call to sort the left half of the array
                MergeSortCars(cars, left, middle, keySelector);

                // Recursive call to sort the right half of the array
                MergeSortCars(cars, middle + 1, right, keySelector);

                // Merge the sorted halves
                MergeCars(cars, left, middle, right, keySelector);
            }
        }

        private void MergeCars<T>(List<Car> cars, int left, int middle, int right, Func<Car, T> keySelector) where T : IComparable<T>
        {
            // Calculate the sizes of the two subarrays to be merged
            int n1 = middle - left + 1;
            int n2 = right - middle;

            // Create temporary lists to hold the left and right subarrays
            List<Car> leftList = new List<Car>(n1);
            List<Car> rightList = new List<Car>(n2);

            // Copy data to temporary lists
            for (int i = 0; i < n1; i++)
                leftList.Add(cars[left + i]);

            for (int j = 0; j < n2; j++)
                rightList.Add(cars[middle + 1 + j]);

            // Merge the two subarrays back into the original array
            int k = left; // Initial index of merged subarray
            int x = 0, y = 0; // Initial indices of the left and right subarrays

            while (x < n1 && y < n2)
            {
                // Compare elements from the left and right subarrays and merge them in sorted order
                if (keySelector(leftList[x]).CompareTo(keySelector(rightList[y])) <= 0)
                {
                    cars[k] = leftList[x];
                    x++;
                }
                else
                {
                    cars[k] = rightList[y];
                    y++;
                }
                k++;
            }

            // Copy any remaining elements from the left subarray
            while (x < n1)
            {
                cars[k] = leftList[x];
                x++;
                k++;
            }

            // Copy any remaining elements from the right subarray
            while (y < n2)
            {
                cars[k] = rightList[y];
                y++;
                k++;
            }
        }
        private bool isAscendingOrder = true; // This is so if we click the button twice it flips between ascending and decending order

        private void SortMakeBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformMergeSort(car => car.Make); // sort by make
        }

        private void SortModelBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformMergeSort(car => car.Model); // sort by model
        }

        private void SortYearBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformMergeSort(car => car.Year); // sort by year
        }

        private void SortPriceBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformMergeSort(car => car.Price); // sort by price
        }

        private void SortMileageBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformMergeSort(car => car.Mileage); // sort by mileage
        }

        private void PerformMergeSort<T>(Func<Car, T> keySelector) where T : IComparable<T>
        {
            // Toggle the sort order
            isAscendingOrder = !isAscendingOrder;

            // Perform Merge Sort directly on the ObservableCollection
            List<Car> sortedCars = new List<Car>(cars);
            MergeSortCars(sortedCars, 0, sortedCars.Count - 1, keySelector);

            // If the order is descending, reverse the sorted list
            if (!isAscendingOrder)
            {
                sortedCars.Reverse();
            }

            // Clear and repopulate the ObservableCollection with the sorted list
            cars.Clear();
            foreach (Car car in sortedCars)
            {
                cars.Add(car);
            }

            CarLV.Items.Refresh();
        }


        private void SaveCSVBtn_Click(object sender, RoutedEventArgs e) // Save to CSV file
        {
            if (string.IsNullOrEmpty(NewFileTxt.Text))
            {
                MessageBox.Show("Please enter a valid CSV Name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                string filePath = NewFileTxt.Text + ".csv"; // The file name is going ot be what ever is in the newfiletxt.text 
                WriteTransactions(filePath);
                NewFileTxt.Clear();
            }
        }
        public void WriteTransactions(string filePath) // Saving the observation list into excel CSV file. 
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(writer, ci))
            {
                csvWriter.WriteRecords(cars);
                writer.Flush();
            }
        }
        
        public void PreloadCars() // Pre Loads
        {
            cars.Add(new Car("Toyota", "Camry", 2022, 25000.50m, 15000));
            cars.Add(new Car("Honda", "Accord", 2021, 28000.75m, 18000));
            cars.Add(new Car("Ford", "Fusion", 2023, 22000.25m, 12000));
            cars.Add(new Car("Chevrolet", "Malibu", 2022, 27000.00m, 16000));
            cars.Add(new Car("Nissan", "Altima", 2023, 26000.80m, 14000));
            cars.Add(new Car("BMW", "3 Series", 2021, 40000.00m, 12000));
            cars.Add(new Car("Mercedes-Benz", "C-Class", 2022, 42000.50m, 13000));
            cars.Add(new Car("Audi", "A4", 2020, 38000.25m, 16000));
            cars.Add(new Car("Tesla", "Model 3", 2023, 50000.00m, 8000));
            cars.Add(new Car("Subaru", "Legacy", 2022, 26000.75m, 20000));
            cars.Add(new Car("Dodge", "Charger", 1970, 50000.00m, 0));
            cars.Add(new Car("Chevrolet", "Chevelle SS", 1970, 60000.75m, 0));
            cars.Add(new Car("Toyota", "Supra", 1995, 45000.25m, 0));
            cars.Add(new Car("Mitsubishi", "Eclipse", 1995, 30000.00m, 0));
            cars.Add(new Car("Honda", "S2000", 2001, 35000.80m, 0));
            cars.Add(new Car("Nissan", "Skyline GT-R R34", 1999, 80000.00m, 0));
            cars.Add(new Car("Ford", "GT40", 1969, 90000.50m, 0));
            cars.Add(new Car("Mazda", "RX-7", 1993, 25000.25m, 0));
            cars.Add(new Car("Plymouth", "Road Runner", 1970, 55000.00m, 0));
            cars.Add(new Car("Chevrolet", "Camaro Z28", 1979, 70000.75m, 0));
            cars.Add(new Car("Dodge", "Charger Daytona", 1969, 75000.25m, 0));
            cars.Add(new Car("Ford", "Mustang Boss 429", 1969, 85000.00m, 0));
            cars.Add(new Car("Chevrolet", "Corvette Stingray", 1971, 60000.25m, 0));
            cars.Add(new Car("Buick", "Grand National", 1987, 40000.00m, 0));
            cars.Add(new Car("Ford", "Escort RS1600", 1971, 30000.75m, 0));
            cars.Add(new Car("BMW", "E92 M3", 2008, 45000.25m, 0));
            cars.Add(new Car("Toyota", "GT86", 2012, 35000.00m, 0));
            cars.Add(new Car("Mercedes-Benz", "AMG GT", 2015, 120000.50m, 0));
            cars.Add(new Car("Plymouth", "Barracuda", 1970, 70000.25m, 0));
            cars.Add(new Car("Chevrolet", "Impala", 1967, 55000.00m, 0));
            cars.Add(new Car("Toyota", "Camry", 2022, 25000.50m, 15000));
            cars.Add(new Car("Honda", "Accord", 2021, 28000.75m, 18000));
            cars.Add(new Car("Ford", "Fusion", 2023, 22000.25m, 12000));
            cars.Add(new Car("Chevrolet", "Malibu", 2022, 27000.00m, 16000));
            cars.Add(new Car("Nissan", "Altima", 2023, 26000.80m, 14000));
            cars.Add(new Car("BMW", "3 Series", 2021, 40000.00m, 12000));
            cars.Add(new Car("Mercedes-Benz", "C-Class", 2022, 42000.50m, 13000));
            cars.Add(new Car("Audi", "A4", 2020, 38000.25m, 16000));
            cars.Add(new Car("Tesla", "Model 3", 2023, 50000.00m, 8000));
            cars.Add(new Car("Subaru", "Legacy", 2022, 26000.75m, 20000));
            cars.Add(new Car("Hyundai", "Elantra", 2022, 20000.00m, 18000));
            cars.Add(new Car("Kia", "Optima", 2021, 23000.50m, 16000));
            cars.Add(new Car("Mazda", "Mazda6", 2023, 28000.25m, 12000));
            cars.Add(new Car("Volkswagen", "Passat", 2022, 24000.00m, 17000));
            cars.Add(new Car("Ford", "Mustang", 2021, 35000.75m, 10000));
            cars.Add(new Car("Chevrolet", "Camaro", 2023, 37000.25m, 8000));
            cars.Add(new Car("Dodge", "Charger", 2022, 32000.00m, 12000));
            cars.Add(new Car("Jeep", "Grand Cherokee", 2022, 40000.50m, 15000));
            cars.Add(new Car("Subaru", "Outback", 2021, 28000.25m, 20000));
            cars.Add(new Car("Toyota", "RAV4", 2023, 29000.00m, 12000));
            cars.Add(new Car("Honda", "CR-V", 2022, 30000.75m, 18000));
        }
    }
}
