 # Introduction
---------------------------------------------------------------------------------------------------------------
  The Electronic Sign-in Sheet (ELSO) is a web-based application used to record meeting attendance.
  The ELSO application allows the creation of an electronic attendance record, saving time and resources by eliminating the need to manually record and transcribe attendance using paper sign-in sheets.
# Technical Requirements
  ---------------------------------------------------------------------------------------------------------------
   The ELSO application is coded in C# and makes use of the following technologies:
 * MVC (Model, View and Controller) Architecture.
 * LINQ Query – this extends the scope of query expressions and allows the application to extract and process data from the database.
 * Responsive design – the application actively responds to a user's behavior and environment.
 * ASP.net Web API – used to build HTTP service/API calls.
 
 
# Project Modules
  ---------------------------------------------------------------------------------------------------------------
 * ELSO enables users to create a one-day meeting or event, and provide a link to attendees, allowing them to enter their Personal Identification Number (PIN) or email address to record their attendance quickly and easily. 
 * Meeting/event organizers are able to view their meeting attendee list within the application or export the list, along with meeting details, to an Excel or PDF document.
 * ELSO provides users the ability to register for a meeting or event, if the meeting organizer enables that option.
 * ELSO allows guests (outside the network for which the application validates a user PIN) to record their attendance using an email address.
 
# Software Required
  ---------------------------------------------------------------------------------------------------------------
 * Visual Studio 2015 (Integrated development environment)
 * Team Foundation Server (Source Code Management)
 * GIT Repository (Version Control System)
 * SQL Server 2012 (Database)
 
 
  # Project Details
  ---------------------------------------------------------------------------------------------------------------
  The ELSO application is comprised of loosely-coupled components, which saves time in implementation and allows for flexibility in adding features or fixing issues.
  They are each stored in this repository as separate projects: 
 * The presentation folder has the ELSO.Web project, which takes care of Views (UI for the application) and controls the request/response.
 * The Business folder has the ELSO.Services project, which takes care of the business logic of the application.
 * The Data Access Layer (DAL) folder has ELSO.Data, a layer which delegates requests to the database and ELSO.Model, which performs database operations.
 
 
# Project Dependencies
  ---------------------------------------------------------------------------------------------------------------
 * ELSO also uses Entity Framework Database First, which allows you to reverse engineer a model from an existing database. For more details: https://msdn.microsoft.com/en-us/library/jj206878(v=vs.113).aspx
 * ELSO contains custom code to validate/sign in Social Security Administration (SSA) users using their PIN. You can modify this code to check your own internal PINs, or bypass this check to require email-based sign-in.
 
## Third Party Libraries
 
 * Jquery(datetimepicker,Datatable)
 * Bootstrap
 * Moment.JS
 * Jquery Validation
 
 
# Installation/Getting Started
  ---------------------------------------------------------------------------------------------------------------
 * Download the latest version from this repository
 * Clone the repository