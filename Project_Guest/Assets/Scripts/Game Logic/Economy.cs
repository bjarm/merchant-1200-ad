using System.Collections;
using System.Collections.Generic;


public class Economy
{
    public void TradeWithCity(City city, Dictionary<Product, int> goods)
    {
        // Обращаемся к БД
        // Находим город в котором произошла операция
        // Из словаря (ключ: товар, значение: баланс товара) берем значения балансов товаров и прибавляем к значениям товаров (на складе города) в БД
        // Те же балансы, взятые со знаком минус прибавляются к значениям товаров в БД, которые отвечают за груз каравана игрока
    }   

}
