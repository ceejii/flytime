Fly Time
========

Overview
--------
An application or web-service that parses hour by hour forecasts from weather service yr.no and uses user-options to determines what hours are flyable. It's intention is to find flyable hours for rc aircrafts but it can be used for other uses that you can think of.

There is currently no binary release of Fly Time.

Technology
----------
It is developed in C# with MVC and ASP.NET for the web service. Visual Studio solution (for Visual Studio Community 2013) is also checked in.

Application
-----------
The project FlyableHours/YrXmlParser is set to build a dll used by the web service. But if you change the project "Output type" from "Class library" to "Console application" you can run it stand-alone. I would suggest running it as a scheduled task. You can set it up to send you email notifications using gmail when it is run. You need to setup email options in App.config in the project root. In this file you also configure the parameters for the search like minimum temperature, max wind speed etc. You need to provide the yr.no URL for the location as a startup parameter for the console application.

Web application
---------------
The FlyableHoursWeb project is a MVC and ASP.NET web application. The start page has a search for a single location where you can also specify some parameters that makes a flyable hour for you. There is also a Slopes page where you can get flyable hours for several slopes on one page. This page also takes wind direction into consideration.
