# Maverick
Venturing into the unknown, we are pioneers of a new era, breaking free from the confines of the past and embracing the limitless possibilities of the future.

# The Vision
As trailblazers in this new era, we have dared to venture into the unknown, shattering the confines of the past and embracing the boundless possibilities of the future. Every challenge we face reveals new opportunities and possibilities, motivating us to push the boundaries and create something truly extraordinary.

We are the dreamers, the thinkers, the creators of a new tomorrow. 
The future belongs to those who dare to dream big and have the courage to pursue their vision.

Let us embrace this exciting journey and use our collective imagination to build something truly innovative.
We know that the road ahead may be bumpy, but we also know that we have the creativity and the drive to overcome the obstacles.

Our path may be uncharted, but we move forward with unwavering determination, driven by our passion to create something truly groundbreaking.
We leave behind the old ways and embrace a new paradigm of creativity and innovation.

Together, we harness the power of our imaginations to build something remarkable. 
we must dare to dream and endeavor to make it a reality.
The future is ours for the taking and we are ready to make our mark on the world.

//Marco Villegas



## WarGamesAPI

### Prerequisites
Before you can run this application, you must have the following software installed on your machine:

* .NET 6.0 SDK
* Microsoft SQL Server
* You'll also need a code editor, such as Visual Studio Code or Visual Studio 2019.

Make sure that your environment meets the minimum requirements for running .NET 6.0.

You should also have a basic understanding of C# and SQL, as well as experience working with RESTful APIs and Entity Framework Core.

### Installation
* Clone this repository to your local machine using git clone [https://github.com/Marco30/Maverick.git].
* In Visual Studio, open Package Manager Console and navigate to the project directory. Run the command dotnet restore. This will restore all NuGet packages.
* Build the solution by clicking on "Build Solution" in the Build menu, or by pressing Ctrl + B.
* Start the application by pressing F5 or clicking on "Start Debugging" in the Debug menu.

### Database Configuration
* The application uses SQL Server as the database. To configure the database, follow these steps:
1. Open the appsettings.json file.
2. Verify that "DefaultConnection" points to your SQL Server instance name. Most likely you do not have to change anything.
3. Run the following commands in the Package Manager Console to create the database tables: Update-Database. This will prompt Entity Framework to create a database from the migration-files.
