**Module 12: Communicating with Backend Services Using HTTP**

---

### **Practice Exercise**

**Practice 1:**
Do the hands-on from the following URL: [https://angular.io/tutorial/toh-pt6](https://angular.io/tutorial/toh-pt6)

---

### **Assignment Exercises**

**Assignment 1:**
With reference to Day 10 assignment, perform CRUD operations with the Student Admission Form.

---

**Assignment 2:**

* Create a CreateComponent, EditComponent, and DeleteComponent.
* Create a Web API using .NET Core, MS SQL Database, and Entity Framework Core.

---

**Assignment 3:**
Create an Angular service to perform CRUD operations (Create, Read, Update, Delete) on the GoRest API ([https://gorest.co.in/](https://gorest.co.in/)). Implement the following:

* **Read:** Fetch and display a list of users from the API (GET request).

  * Implement pagination and display 10 users per page.
  * Display user details such as name, email, and status.

* **Create:**

  * Implement a form to create a new user and add it to the list (POST request).
  * Validate the form to ensure the name and email are provided, and email is in a valid format.
  * Add a default status of "active" unless specified otherwise.

* **Update:**

  * Allow editing of an existing user's information (name, email, status) and update it via the API (PUT request).
  * Before updating, check if the email already exists for another user. If it does, show a warning that the email is already in use.

* **Delete:**

  * Provide a way to delete a user from the list (DELETE request).
  * Implement a confirmation dialog before deleting a user to prevent accidental deletions.
  * After successful deletion, refresh the user list.

---

**Business Logic:**

* **Active User Filtering:**

  * Add a filter to display only active users by default.
  * Allow toggling between active and inactive users with a button, updating the list accordingly.

* **Error Handling:**

  * Ensure proper error handling for each API request (e.g., if a user tries to create a user with an existing email, show a specific error message).
  * Display a success message after successful operations (e.g., user creation, update, or deletion).

