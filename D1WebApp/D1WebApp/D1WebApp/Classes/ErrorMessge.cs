using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.Utilities
{
    public class ErrorMessge
    {
        public static string GetErrorMessage(int value)
        {
            string errormessage = "";
            if (value == 1)
            {
                errormessage = "User not found.";
            }
            else if (value == 2)
            {
                errormessage = "Product not found.";
            }
            else if (value == 3)
            {
                errormessage = "Product Size not found.";
            }
            else if (value == 4)
            {
                errormessage = "Supplier not found.";
            }
            else if (value == 5)
            {
                errormessage = "Quantity Required";
            }
            else if (value == 6)
            {
                errormessage = "Rate Required";
            }
            else if (value == 7)
            {
                errormessage = "you are not paid member please contact D1WebApp. ";
            }
            else if (value == 8)
            {
                errormessage = "Product profile reach maximum no 10";
            }
            else if (value == 9)
            {
                errormessage = "Product profile name duplicate found.";
            }
            else if (value == 10)
            {
                errormessage = "User duplicate found.";
            }
            else if (value == 11)
            {
                errormessage = "You have already sent response of this enquiry.";
            }
            else if (value == 12)
            {
                errormessage = "Please select Delivery location.";
            }
            else if (value == 13)
            {
                errormessage = "Your company can not found contact D1WebApp";
            }
            else if (value == 14)
            {
                errormessage = "Company must be certify";
            }
            else if (value == 15)
            {
                errormessage = "Please select sell location.";
            }
            else if (value == 16)
            {
                errormessage = "Your daily seller market update count is over";
            }
            else if (value == 17) // company ID is not create
            {
                errormessage = "your detail not verify please wait some time contact on D1WebApp support.";
            }
            else if (value == 18)
            {
                errormessage = "Please select folder name";
            }
            else if (value == 19)
            {
                errormessage = "Folder Name is required";
            }
            else if (value == 20)
            {
                errormessage = "Please select Enquiry";
            }
            else if (value == 21)
            {
                errormessage = "Folder Name is required";
            }
            else if (value == 22)
            {
                errormessage = "Company is require";
            }
            else if (value == 23)
            {
                errormessage = "Specify favourite or contact";
            }
            else if (value == 24)
            {
                errormessage = "Please select company";
            }
            else if (value == 25)
            {
                errormessage = "Please select folder name";
            }
            else if (value == 26)
            {
                errormessage = "Please select Product";
            }
            else if (value == 27)
            {
                errormessage = "Please select company";
            }
            else if (value == 28)
            {
                errormessage = "Please provide Reference no";
            }
            else if (value == 29)
            {
                errormessage = "Product not found";
            }
            else if (value == 30)
            {
                errormessage = "Please Provide one product rate with unit";
            }
            else if (value == 31)
            {
                errormessage = "Buyer not found.";
            }
            else if (value == 32)
            {
                errormessage = "It's User have not Rights.";
            }
            else if (value == 33)
            {
                errormessage = "It's for company userid not found.";
            }
            else
            {
                errormessage = "None";
            }
            return errormessage;
        }
    }
}