using System;
using System.Collections.Generic;

class Car // A car to be on the website needs a make, model, year, price, and mileage listed
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }

    public Car(string make, string model, int year, decimal price, int mileage)
    {
        Make = make;
        Model = model;
        Year = year;
        Price = price;
        Mileage = mileage;
    }

    public Car()
    {
    }

    public override string ToString() // override is used for if I go deaper and want to print out the actual information like a Carfax type thing
    {
        return $"{Year} {Make} {Model} - ${Price}, {Mileage} miles";
    }
 
}



