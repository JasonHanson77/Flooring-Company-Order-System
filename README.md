# Flooring-Company-Order-System

A console app that emulates an ordering and inventory system that may be found at a flooring company.  The app runs in two modes "Prod" and "Test".  

The test mode utilizes an in-memory order repository that assigns new order numbers based on the previous max integer-based order number plus + 1.

The production mode writes orders to text files that are named base on the current day's date in integer format (ex Jan 1 2017 = 112017.text).  Each order placed in each file is assigned the same as the test repository based on max previous int order number.

The user is given the basic CRUD functionality as well as the ability to view all orders for a specific date.

