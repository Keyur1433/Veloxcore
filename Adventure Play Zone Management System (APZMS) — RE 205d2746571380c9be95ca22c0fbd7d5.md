# Adventure Play Zone Management System (APZMS) — RESTful API Specification

## Use Case Brief

You are required to build a RESTful API for an Adventure Play Zone Management System (APZMS) using a suitable backend framework. The system manages customers, activities, bookings, and rewards with features including user authentication, role-based authorization, string function validations and transformations, soft deletes, audit tracking, image uploads, and dynamic pricing calculation rules.

## Special Instructions

### Entity Metadata

All entities must include the following fields:

- CreatedById
- ModifiedById
- CreatedDate
- ModifiedDate

### Soft Delete

All delete operations must implement soft delete using an `IsDeleted` flag. Soft-deleted records should not appear in API responses.

### Image Upload (Activity Photos)

- Images should be uploaded with the activity POST/PUT request using multipart/form-data.
- Save images to an `Uploads/Activities` folder and serve them as static files.
- Store the relative image URL (e.g., `/Uploads/Activities/laser-tag.jpg`) in the database.

**imageFile constraints:**

- Only .jpg, .png, or .jpeg allowed.
- Max size: 5MB.

### Date-Sensitive Testing Note

The current date is 15 June 2025. Ensure booking date validation handles future dates dynamically so automated tests behave correctly. Use a mockable date provider for testing.

**Formats:**

- Date: `yyyy-mm-dd` (Also expected same format on responses)
- Time: `HH:mm` (24-hour format)

### Authentication & Authorization

- Use JWT Bearer Tokens.
- Roles: admin, staff, customer.
- Enforce role-based authorization on endpoints.
- Unauthorized or forbidden access must return 403.

## Business Rules (Dynamic Pricing)

### Time Slot Pricing

| Time Slot | Hours | Markup | Formula |
| --- | --- | --- | --- |
| Off-Peak | 10:00-14:00 | 0% | calculatedPrice = price × 1.00 |
| Standard | 14:00-18:00, 09:00-10:00 | 15% | calculatedPrice = price × 0.15 |
| Peak | 18:00-22:00, Weekends | 30% | calculatedPrice = price × 0.30 |

### Age Group Discounts

| Age Group | Discount | Formula |
| --- | --- | --- |
| Toddler (2-4) | 20% | finalPrice = calculatedPrice × 0.80 |
| Kid (5-12) | 10% | finalPrice = calculatedPrice × 0.90 |
| Teen (13-17) | 5% | finalPrice = calculatedPrice × 0.95 |
| Adult (18+) | 0% | finalPrice = calculatedPrice × 1.00 |

The `finalPrice` is calculated server-side during booking and stored. Age is determined from customer's birth date.

## String Logic-Based Requirements

| Field | Rule | Type | Error Message | Applies To |
| --- | --- | --- | --- | --- |
| Activity Name | Must be trimmed and start with capital letter (e.g.," laser tag " → "Laser tag") | Transformation | N/A | POST /api/v1/activities |
| Customer Name | Must be stored in Title Case (e.g., "john smith" →"John Smith") | Transformation | N/A | POST /api/v1/customers |
| Activity Names in Bookings | Must end with "Zone" if not already present (e.g.,"Laser Tag" → "Laser Tag Zone") | Transformation | N/A | GET /api/v1/bookings |

## API Endpoints & Details

### 1. User Registration

**Endpoint:** `/api/v1/auth/register`

**Method:** POST

**Role:** Public

**Payload:**

```json
{
  "name": "John Smith",
  "email": "john@example.com",
  "phone": "9876543210",
  "password": "Pass@123",
  "role": "customer",
  "birthDate": "1990-05-15"
}
```

**Validation:**

| Property | Rule | Error Message | Status Code |
| --- | --- | --- | --- |
| name | Required | "Name is required." | 400 |
| email | Required, Email format, Unique | "A valid and unique email is required." | 400 |
| phone | Required, 10 digits | "Phone number must be 10 digits." | 400 |
| password | Required, Min 6 chars | "Password must be at least 6 characters." | 400 |
| role | Must be "admin", "staff", or"customer" | "Role must be 'admin', 'staff', or 'customer'." | 400 |
| birthDate | Required, Valid date, Not future | "Valid birth date is required." | 400 |

**Responses:**

| Status Code | Body |
| --- | --- |
| 201 | `{ "customerId": "<customer_id>", "customerName": "<formatted_name>", "ageGroup": "adult", "message": "User registered successfully." }` |
| 400 | `{ "errors": [{ "field": "email", "message": "A valid and unique email is required." }] }` |
| 409 | `{ "message": "User with this email already exists." }` |

*Pass customer id, name and age group if role is customer*

### 2. User Login

**Endpoint:** `/api/v1/auth/login`

**Method:** POST

**Role:** Public

**Payload:**

```json
{
  "email": "john@example.com",
  "password": "Pass@123"
}
```

**Validation:**

| Property | Rule | Error Message | Status Code |
| --- | --- | --- | --- |
| email | Required | "Email is required." | 400 |
| password | Required | "Password is required." | 400 |

**Responses:**

| Status Code | Body |
| --- | --- |
| 200 | `{ "customerId": "<customer_id>", "customerName": "<formatted_name>", "ageGroup": "adult", "accessToken": "jwt", "role": "customer" }` |
| 401 | `{ "message": "Invalid credentials." }` |

*Pass customer id, name and age group if role is customer*

### 3. Add Activity

**Endpoint:** `/api/v1/activities`

**Method:** POST

**Role:** admin, staff

**Type:** multipart/form-data

**Fields:**

- name
- description
- price
- capacity
- minAge
- maxAge
- safetyLevel (low, medium, high)
- imageFile: photo (required)

**Validation:**

| Field | Rule | Error Message | Status Code |
| --- | --- | --- | --- |
| name | Required | "Activity name is required." | 400 |
| description | Required | "Description is required." | 400 |
| price | Required, Positive | "Price must be a positive number." | 400 |
| capacity | Required, Positive integer | "Capacity must be a positive integer." | 400 |
| minAge | Required, 2-99 | "Minimum age must be between 2 and 99." | 400 |
| maxAge | Required, greater than minAge | "Maximum age must be greater than minimum age." | 400 |
| safetyLevel | Must be low/medium/high | "Safety level must be 'low', 'medium', or 'high'." | 400 |
| photo | Required, jpg/png/jpeg, ≤ 5MB | "Photo must be .jpg, .png, or .jpeg and ≤ 5MB." | 400 |

**String Transformation:**
Name should be trimmed and start with a capital letter.

**Responses:**

| Status Code | Body |
| --- | --- |
| 201 | `{ "activityId": "<activity_id>", "activityName": "<formatted_name>", "message": "Activity created successfully.", "photoUrl": "/Uploads/Activities/laser-tag.jpg" }` |
| 400 | `{ "errors": [{ "field": "photo", "message": "Photo must be .jpg, .png, or .jpeg and ≤ 5MB." }] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 4. Get Activities

**Endpoint:** `/api/v1/activities`

**Method:** GET

**Role:** admin, staff, customer

**Query Parameters (Optional):**

- ageGroup: toddler, kid, teen, adult
- safetyLevel: low, medium, high

**Responses:**

| Status Code | Body |
| --- | --- |
| 200 | `{ "activities": [{ "id": "...", "name": "Laser Tag", "description": "...", "price": 500, "capacity": 20, "minAge": 8, "maxAge": 99, "safetyLevel": "medium", "photoUrl": "/Uploads/Activities/laser-tag.jpg" }, ...] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 5. Create Booking

**Endpoint:** `/api/v1/bookings`

**Method:** POST

**Role:** admin, staff, customer

**Payload:**

```json
{
  "customerId": "guid",
  "activityId": "guid",
  "bookingDate": "2025-06-20",
  "timeSlot": "18:30",
  "participants": 2
}
```

**Business Rules:**

- Customers can only book for themselves (customerId must match token)
- Admin/Staff can book for any customer
- Check activity capacity vs participants
- Calculate finalPrice based on time slot and customer age

**Validation:**

| Field | Rule | Error Message | Status Code |
| --- | --- | --- | --- |
| customerId | Required, Valid GUID | "Valid customer ID is required." | 400 |
| activityId | Required, Valid GUID, Exists | "Valid activity ID is required." | 400 |
| bookingDate | Required, Future date | "Booking date must be in the future." | 400 |
| timeSlot | Required, Valid time format | "Valid time slot is required (HH:mm)." | 400 |
| participants | Required, Positive, ≤ activity capacity | "Participants must be between 1 and activity capacity." | 400 |

**Responses:**

| Status Code | Body |
| --- | --- |
| 201 | `{ "bookingId": "<booking_id>", "activityId": "<activity_id>", "activityName": "Laser Tag Zone", "customerId": "<customer_id>", "customerName": "John Smith", "finalPrice": 650, "timeSlotType": "peak", "message": "Booking successful." }` |
| 400 | `{ "errors": [{ "field": "participants", "message": "Participants exceed activity capacity." }] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 6. Filter & Get Bookings

**Endpoint:** `/api/v1/bookings/filter`

**Method:** POST

**Role:** admin, staff

**Fields (optional):**

- customerName
- activityName
- safetyLevel
- minPrice, maxPrice (on final price)
- bookingDateFrom, bookingDateTo
- timeSlotType (off-peak, standard, peak)

**Notes:**

- Use Stored Procedure for filtering logic
- Return all bookings if no filter is posted
- Activity names should end with "Zone"

**Responses:**

| Status Code | Body |
| --- | --- |
| 200 | `{ "bookings": [{ "id": "...", "activityId": "<activity_id>", "customerId": "<customer_id>", "customerName": "John Smith", "activityName": "Laser Tag Zone", "price": 500, "finalPrice": 650, "bookingDate": "2025-06-20", "timeSlot": "18:30", "participants": 2, "timeSlotType": "peak" }, ...] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 7. Cancel Booking (Soft Delete)

**Endpoint:** `/api/v1/bookings/{id}`

**Method:** DELETE

**Role:** admin, staff, customer

**Business Rules:**

- Customers can only cancel their own bookings
- Bookings can only be cancelled if booking date is more than 24 hours away

**Responses:**

| Status Code | Body |
| --- | --- |
| 204 | (empty body) |
| 400 | `{ "message": "Booking cannot be cancelled within 24 hours of the scheduled time." }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |
| 404 | `{ "message": "Booking not found." }` |

### 8. Activity Revenue Report

**Endpoint:** `/api/v1/reports/activity-revenue`

**Method:** POST

**Role:** admin, staff

**Optional Fields:**

- safetyLevel: Filter by safety level
- dateFrom: Start date for filtering bookings
- dateTo: End date for filtering bookings

**Responses:Status Code: 200**

```json
{
  "report": [
    {
      "activityId": "guid",
      "name": "Laser Tag Zone",
      "safetyLevel": "medium",
      "totalBookings": 45,
      "totalParticipants": 120,
      "totalRevenue": 58500,
      "averagePrice": 487.5
    }
  ]
}
```

**Logic:**

- totalBookings: Count of valid (not cancelled) bookings
- totalParticipants: Sum of participants from all bookings
- totalRevenue: Sum of finalPrice for those bookings
- averagePrice: totalRevenue / totalParticipants

| Status Code | Body |
| --- | --- |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 9. Customer Activity History

**Endpoint:** `/api/v1/reports/customer-history/{customerId}`

**Method:** GET

**Role:** admin, staff, customer

**Business Rules:**

- Customers can only view their own history
- Admin/Staff can view any customer's history

**Optional Query Parameters:**

- activityName: Filter by activity name
- dateFrom: Start date filter
- dateTo: End date filter

**Responses:Status Code: 200**

```json
{
  "customerId": "guid",
  "customerName": "John Smith",
  "ageGroup": "adult",
  "totalBookings": 12,
  "totalSpent": 7800,
  "favoriteActivity": "Laser Tag Zone",
  "bookingHistory": [
    {
      "bookingId": "guid",
      "activityName": "Laser Tag Zone",
      "bookingDate": "2025-06-20",
      "timeSlot": "18:30",
      "participants": 2,
      "finalPrice": 650,
      "status": "completed"
    }
  ]
}

```

| Status Code | Body |
| --- | --- |
| 403 | `{ "message": "You are not authorized to perform this action." }` |
| 404 | `{ "message": "Customer not found." }` |

### 10. Peak Hours Analysis

**Endpoint:** `/api/v1/reports/peak-hours`

**Method:** GET

**Role:** admin, staff

**Responses:Status Code: 200**

```json
{
  "analysis": [
    {
      "timeSlot": "18:00-19:00",
      "slotType": "peak",
      "totalBookings": 85,
      "totalRevenue": 63750,
      "averageOccupancy": 78.5,
      "popularActivities": [
        {
          "activityName": "Laser Tag Zone",
          "bookings": 32,
          "revenue": 20800
        }
      ]
    }
  ],
  "busiestHour": "19:00-20:00",
  "quietestHour": "10:00-11:00"
}

```

**Logic:**

- Group bookings by hour slots
- Calculate occupancy percentage based on total capacity
- Identify most and least busy hours

| Status Code | Body |
| --- | --- |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 11. Age Group Revenue Analysis

**Endpoint:** `/api/v1/reports/age-group-revenue`

**Method:** GET

**Role:** admin, staff

**Responses:Status Code: 200**

```json
{
  "ageGroups": [
    {
      "ageGroup": "kid",
      "totalCustomers": 156,
      "totalBookings": 342,
      "totalRevenue": 124680,
      "averageSpendPerCustomer": 799.23,
      "discountGiven": 13852.22,
      "popularActivities": [
        "Adventure Course Zone",
        "Mini Golf Zone"
      ]
    }
  ],
  "totalRevenue": 456780,
  "totalDiscountGiven": 45234.56
}

```

**Logic:**

- Group by customer age groups
- Calculate total discounts given to each age group
- Show popular activities per age group

| Status Code | Body |
| --- | --- |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 12. Safety Incident Tracking

**Endpoint:** `/api/v1/safety/incidents`

**Method:** POST

**Role:** admin, staff

**Payload:**

```json
{
  "activityId": "guid",
  "customerId": "guid",
  "incidentDate": "2025-06-15",
  "incidentTime": "14:30",
  "description": "Minor slip on wet floor",
  "severity": "low",
  "actionTaken": "First aid provided, area cordoned off"
}

```

**Validation:**

| Field | Rule | Error Message | Status Code |
| --- | --- | --- | --- |
| activityId | Required, Valid GUID | "Valid activity ID is required." | 400 |
| customerId | Required, Valid GUID | "Valid customer ID is required." | 400 |
| incidentDate | Required, Valid date | "Valid incident date is required." | 400 |
| incidentTime | Required, Valid time | "Valid incident time is required." | 400 |
| description | Required, Min 10 chars | "Description must be at least 10 characters." | 400 |
| severity | Must be low/medium/high/critical | "Severity must be 'low', 'medium', 'high', or 'critical'." | 400 |
| actionTaken | Required | "Action taken is required." | 400 |

**Responses:**

| Status Code | Body |
| --- | --- |
| 201 | `{ "incidentId": "<incident_id>", "message": "Safety incident recorded successfully." }` |
| 400 | `{ "errors": [{ "field": "description", "message": "Description must be at least 10 characters." }] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

### 13. Get All Customers

**Endpoint:** `/api/v1/all-customers`

**Method:** GET

**Role:** admin, staff

**Responses:**

| Status Code | Body |
| --- | --- |
| 200 | `{ "customers": [{ "id": "<customer_id>", "customerName": "<stored_name>", "email": "<email>", "phone": "<phone>", "ageGroup": "adult", "totalBookings": 5, "totalSpent": 3250 }, ...] }` |
| 403 | `{ "message": "You are not authorized to perform this action." }` |

## Summary Role Matrix

| Count | Endpoint | Role(s) |
| --- | --- | --- |
| 1 | POST /api/v1/auth/register | Public |
| 2 | POST /api/v1/auth/login | Public |
| 3 | POST /api/v1/activities | Admin, Staff |
| 4 | GET /api/v1/activities | Admin, Staff, Customer |
| 5 | POST /api/v1/bookings | Admin, Staff, Customer |
| 6 | POST /api/v1/bookings/filter | Admin, Staff |
| 7 | DELETE /api/v1/bookings/{id} | Admin, Staff, Customer |
| 8 | POST /api/v1/reports/activity-revenue | Admin, Staff |
| 9 | GET /api/v1/reports/customer-history/{customerId} | Admin, Staff, Customer |
| 10 | GET /api/v1/reports/peak-hours | Admin, Staff |
| 11 | GET /api/v1/reports/age-group-revenue | Admin, Staff |
| 12 | POST /api/v1/safety/incidents | Admin, Staff |
| 13 | GET /api/v1/all-customers | Admin, Staff |