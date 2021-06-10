using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public Date currentDate;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentDate = new Date(1, 1, 1240);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Date
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }
        
        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public string GetDateString()
        {
	        return Day + "." + Month + "." + Year;
        }

        public void ChangeData(int amountOfDaysPassed)
        {
            
        }
    }

    public void ChangeCurrentData(int amountOfDaysPassed)
    {
        
    }

}
