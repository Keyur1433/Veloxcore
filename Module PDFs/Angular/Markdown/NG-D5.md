### **Practice Exercise**

**Practice 1**
*   Do the hands-on from the following url for Reactive Form and Validating Form
    *   https://angular.io/guide/reactive-forms
*   Create A Reactive Form and try to update partially some field using patch value

---

### **Assignment Exercise**

**Assignment 1**
Create a Student Admission Form with the Following Field
*   **Student Information:** control id -> `#studentForm`
    *   firstName: control id -> `#firstName`
    *   middleName: control id -> `#middleName`
    *   lastName: control id -> `#lastName`
    *   DOB: control id -> `#dob`
    *   place of birth: control id -> `#placeOfBirth`
    *   first Language: control id -> `#firstLanguage`
    *   city name: control id -> `#city`
    *   state name: control id -> `#state`
    *   country name: control id -> `#country`
    *   pincode: control id -> `#pin`
*   **Father Information:**
    *   father fullName: control id -> `#fatherFullName`
    *   father email: control id -> `#fatherEmail`
    *   father education qualification: control id -> `#fatherEducation`
    *   father profession: control id -> `#fatherProfession`
    *   father designation: control id -> `#fatherDesignation`
    *   father phone number: control id -> `#fatherPhone`
*   **Mother Information:**
    *   mother fullName: control id -> `#motherFullName`
    *   mother email: control id -> `#motherEmail`
    *   mother education qualification: control id -> `#motherEducation`
    *   mother profession: control id -> `#motherProfession`
    *   mother designation: control id -> `#motherDesignation`
    *   mother phone number: control id -> `#motherPhone`
*   **Emergency Contact:** (formArrayName)
    *   relation: control id -> `"emergencyContact' + {i}"`
        *   *Dropdown with value (Sibling, Uncle, Aunt, Grandparent)*
    *   contact no: control id -> `"emergencyNumber' + {i}"`
*   **Buttons:**
    *   Add More: control id -> `#add-emergencyContacts` (Add more Emergency Contact)
    *   Submit: control id -> `#submit`

**validations:**
1.  Required -> {control name} is required (for all fields)
2.  Email -> {control name} email format is invalid (for email type)
3.  Phone number -> {control name} number should 10 characters (for phone number pattern)