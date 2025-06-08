# WPF Employee Management Application

## Project Overview

This is an **interactive .NET WPF application** designed for managing hierarchical employee data. The project was developed as part of a university "Technology Platforms" course. It provides a user-friendly graphical interface with editable grids, a live inspector panel, and robust data import/export capabilities, including XML and HTML formats.

The project was a collaborative effort by [@JanBancerewicz](https://github.com/JanBancerewicz), [@gre-v-el](https://github.com/gre-v-el), and [@ThreeCoffees](https://github.com/ThreeCoffees).

## Features

The application offers a comprehensive set of functionalities to interact with employee data, accessible through an intuitive user interface, which recursively showcases a tree structure of the data:

![Image](https://github.com/user-attachments/assets/673f9189-284d-445a-9b0a-d8ed456fddf1)


### Data Management & Persistence
* **Generate Data**: Populate the application with sample, hierarchical employee data.
* **Export/Import Data**:
    * **Export**: Save the current employee dataset to a file.
    * **Import**: Load employee data from a file, allowing for easy data persistence and sharing.
* **Specialized Save Formats**:
    * **Save as XPath**: Export data as an XML document, structured for easy querying using XPath.
    * **Save as HTML**: Export data as an HTML table, suitable for web browser viewing or simple reporting.

### Employee Data View and Interaction
* **Main Data Grid (`PracownicyDataGrid`)**:
    * Displays core employee information: `Imie` (First Name), `Nazwisko` (Last Name), `Staż` (Seniority), `Pensja` (Salary), `Stanowisko` (Position), `Premia` (Bonus), `Ocena` (Rating), and `Wykształcenie` (Education Level).
    * **Editable**: Allows direct editing of employee details within the grid.
    * **Add/Delete Rows**: Supports adding new employee records and removing existing ones directly from the grid.
    * **Sorting**: Click on column headers to sort employee data.
    * **Hierarchical View (Row Details)**: When an employee is selected, their direct subordinates can be viewed in a nested `DataGrid` within the row details, allowing for navigation through the organizational hierarchy.
* **Employee Inspector Panel**:
    * Provides a detailed view of the currently selected employee, including all their properties and calculated total number of subordinates (`Liczba wszystkich podwładnych`).
    * This panel dynamically updates as different employees are selected.

### Search and Filtering
* **Generic Search Toolbar**:
    * **Property Selection**: Choose an employee property (e.g., `Imie`, `Nazwisko`, `Staż`, `Pensja`) from a dropdown list to search by.
    * **Value Input**: Enter the specific value to search for.
    * **Search Button**: Filters the main `DataGrid` to display only employees matching the criteria.
* **In-Panel Actions (Recursive Operations)**:
    * **Sort by Surname (Recursive)**: Sorts the selected employee's direct subordinates and their sub-subordinates by surname.
    * **Sort by Salary**: Sorts the selected employee's direct subordinates by salary.
    * **Find by Name (Recursive)**: Recursively searches for employees with a specific name within the selected employee's entire subordinate tree.
    * **Find by Seniority**: Searches for employees with a specific seniority within the selected employee's direct subordinates.

![Image](https://github.com/user-attachments/assets/f070c57f-115a-463b-bcea-065c83253383)

### Utilities & Application Control
* **Expand/Collapse All**: Convenient buttons to toggle the visibility of all nested subordinate grids for a quick overview or detailed inspection.
* **Predefined Queries (`Zapytanie 1`, `Zapytanie 2`)**: Execute specific, custom queries or reports on the employee data (functionality implemented in code-behind).
* **Version Info**: Display basic application version details.

## Technology Stack

* **.NET Framework / .NET Core**: The underlying platform for the application.
* **C#**: The primary programming language.
* **WPF (Windows Presentation Foundation)**: For building the rich desktop graphical user interface.
* **XAML**: For defining the UI structure and appearance.

## How to Run

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/JanBancerewicz/dotnet-employee-management.git](https://github.com/JanBancerewicz/dotnet-employee-management.git)
    cd your-wpf-repo-name
    ```

2.  **Open in Visual Studio:**
    Open the `.sln` (solution) file in Visual Studio (2019 or later recommended).

3.  **Build the Project:**
    Build the solution (Build > Build Solution or `Ctrl+Shift+B`). This will compile the C# code and XAML.

4.  **Run the Application:**
    Start the application (Debug > Start Debugging or `F5`).

## Program Objective

This WPF application serves as a practical demonstration of several key concepts in desktop application development and data management:

* **WPF UI Development**: Illustrates building interactive and dynamic user interfaces using XAML and C#.
* **Data Binding**: Showcases effective use of data binding for seamless interaction between UI elements and underlying data models (`Pracownik`).
* **Hierarchical Data Display**: Demonstrates how to present and interact with nested, hierarchical data structures within `DataGrid` controls.
* **Data Serialization/Deserialization**: Provides examples of exporting and importing data in various formats (XML, HTML).
* **Basic Search and Sort Algorithms**: Implements functionalities for filtering and organizing data.
* **Event Handling**: Shows how to respond to user interactions via events in a WPF application.

## Authors

* [@JanBancerewicz](https://github.com/JanBancerewicz)
* [@gre-v-el](https://github.com/gre-v-el)
* [@ThreeCoffees](https://github.com/ThreeCoffees)
