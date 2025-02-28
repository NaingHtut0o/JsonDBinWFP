using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthTest.Utilities
{
    public static class MessageClass
    {
        // Greeting
        public static string WelcomeMsg = "Welcome to スマート ヘルス";
        public static string GoodByeMsg = "Farewell, See you next time";

        // Task
        public static string ButtonStyleChangeMsg = "Changing Button Style...";
        public static string ButtonStyleResetMsg = "Resetting Button Style...";
        public static string NavigatePageMsg = "Navigating to Page...";
        public static string DarkModeMsg = "Changing to Dark Mode...";
        public static string LightModeMsg = "Changing to Light Mode...";
        public static string UpdateDataMsg = "Updating Data...";

        // Error
        public static string ButtonStyleResetErr = "Error While Reseting Button Style!!!";
        public static string NavigatePageErr = "Navigating Failed!!!";
        public static string DarkModeErr = "Dark Mode Failed!!!";
        public static string LightModeErr = "Light Mode Failed!!!";
        public static string UpdateDataErr = "Updating Data Failed!!!";

        // Success
        public static string UpdateSucMsg = "Successfully Updated...";
        public static string DeleteSucMsg = "Successfully Deleted...";

        // Info
        public static string ConfirmDeleteMsg = "Are you sure you want to delete ";

        // Warning
        public static string UnfillDataMsg = "You have to fill all data.";

        // Message Boxes Titles
        public static string ConfirmDeleteTitle = "Confirm Delete";
        public static string UnfillDataTitle = "Fill All Data";
        public static string SuccessTitle = "Success Task";
    }
}
