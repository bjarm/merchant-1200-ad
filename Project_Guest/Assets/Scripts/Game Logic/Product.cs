using System.Collections;
using System.Collections.Generic;

public class Product
{
    private string name; // Название товара
    private double productValue; // Стоимость

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public double ProductValue
    {
        get { return productValue; }
        set { productValue = value; }
    }
}
