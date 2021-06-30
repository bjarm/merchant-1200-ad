using System;

public class Date
{
	public int day;
	public Month month;
	public int year;
 
	public enum Month
	{
		January = 1,
		February,
		March,
		April,
		May,
		June,
		July,
		August,
		September,
		October,
		November,
		December
	}
	public Date(int day, int month, int year)
	{
		this.year = year;
 
		if (!IsCorrectMonth(month))
			throw new Exception("Incorrect month");
 
		this.month = (Month)month;
 
		if (!IsCorrectDay(day))
			throw new Exception("Incorrect day");
 
		this.day = day;
	}
 
	private bool IsCorrectMonth(int month)
	{
		return (month >= 1 && month <= 12) ? true : false;
	}
 
	private bool IsCorrectDay(int day)
	{
		return (day >= 1 && day <= DayInMonth()) ? true : false;
	}
 
	private bool IsLeapYear()
	{
		if (year % 400 == 0 || year % 4 == 0 && year % 100 != 0)
			return true;
		return false;
	}
 
	private int DayInMonth()
	{
		switch (month)
		{
			case Month.February:
				return IsLeapYear() ? 29 : 28;
			case Month.April:
			case Month.June:
			case Month.September:
			case Month.November:
				return 30;
			default:
				return 31;
		}
	}
 
	public void AddDays(int days)
	{
		day += days;
		int dayInCurrentMonth;
		while (day > (dayInCurrentMonth = DayInMonth()))
		{
			day -= dayInCurrentMonth;
			NextMonth();
		}
	}
	
	public void NextMonth()
	{
		if ((int)++month > 12)
		{
			month = Month.January;
			NextYear();
		}
	}
 
	public void NextYear()
	{
		year++;
	}
	
	public override string ToString()
	{
		return $"{day:d2}.{(int)month:d2}.{year}";
	}
}
