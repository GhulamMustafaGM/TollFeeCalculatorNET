using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorNET
{
    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public class TollCalculator
    {
        // Instead of hardcoding MaxFeePerDay and TimeInterval values, we will get them from App.config file. We can easily change these values in future if required
        // we declare these variables in the start to use them in any method of this class instead of creating multiple times in different methods 

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Calculate the toll fee for a day
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            try
            {
                // Calling IsTollFreeVehicle() method for all dates. It will check here only and remove this call from GetTollFee(dates).
                if (dates.Length == 0 || vehicle.IsTollFreeVehicle())
                    return 0;
                DateTime intervalStart = dates[0];
                int totalFee = GetTollFee(intervalStart);

                // Maximum fees of 60 per day. It will take it from web.config.
                int maxFee = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxFeesPerDay"]); // MaxFee = 60 SEK

                foreach (DateTime date in dates)
                {
                    // Log files(using Log4Net dll) is used to store database inorder to find the issues occured for which particular date in progress. 
                    logger.Info(" The calculation started for date " + date);

                    // Difference between intervals in minutes.
                    double minutes = (date - intervalStart).TotalMinutes;

                    int timeInterval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeInterval"]); // TimeInterval value 60 minutes
                    if (minutes > timeInterval)
                    {
                        intervalStart = date; // We reset the intervalTime to current time here to calculate the next intervalTime.
                        int currentFee = GetTollFee(date); // Calculate the current fee.
                        totalFee += currentFee;
                    }

                    // Break loop if bill process when cross over 60.
                    if (totalFee > maxFee)
                        break;
                }

                if (totalFee > maxFee) totalFee = maxFee;  // Difference between intervals in minutes
                return totalFee;
            }
            catch (Exception ex)
            {
                // Log files exception
                logger.Info("Exception in GetAadharDetails Method. Exception is " + ex.Message);
                throw ex;
            }
        }

        // Get the toll fee for relevant intervalTime
        private int GetTollFee(DateTime date)
        {
            // Check if the current day is a holiday or toll free date then return 0
            if (IsTollFreeDate(date)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if ((hour == 6 && minute >= 0 && minute <= 29) ||
               (hour == 18 && minute >= 0 && minute <= 29) ||
               (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59))
                return 9;

            if ((hour == 6 && minute >= 30 && minute <= 59) ||
               (hour == 17 && minute >= 0 && minute <= 59) ||
               (hour == 15 && minute >= 0 && minute <= 29) ||
               (hour == 8 && minute >= 0 && minute <= 29))
                return 16;

            if ((hour == 7 && minute >= 0 && minute <= 59) ||
               (hour == 15 && minute >= 30 || hour == 16 && minute <= 59))
                return 22;

            return 0;
        }

        // Check if the current date is a tollfee free date
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            // We should avoid hardcoded values here. We should have some holidays database table from where we can get holidays data. 
            // Here we do have any other option to proceed,so we are using this as it is. 
            if (year == 2021)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
