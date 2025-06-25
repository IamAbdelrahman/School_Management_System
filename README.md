# 🎓 School Management System

This is a full-featured school management system built using **ASP.NET Core MVC** and **Entity Framework Core**, designed for educational institutions to manage their academic operations efficiently. It allows role-based access for administrators, teachers, and students, with complete tracking of courses, classes, enrollments, exams, and student performance.

---

## 🧭 Project Overview

The School Management System provides a modular and scalable architecture to handle all essential school operations, including:

- 🧑‍🎓 **Student Management**: Register, edit, assign students to classes, and track their academic journey.
- 🧑‍🏫 **Teacher Management**: Assign teachers to departments, track the subjects they teach, and manage exams.
- 🏫 **Class Organization**: Organize students into classes with designated class advisors (teachers).
- 📘 **Course Management**: Create and manage academic courses linked to departments and taught by teachers.
- 🧾 **Enrollment System**: Allow students to enroll in multiple courses, with historical tracking.
- 📝 **Exam & Grading**: Assign multiple types of exams to each course and track individual student grades through a dedicated grading module.
- 🔐 **Authentication & Authorization**: Built-in role-based access control for Admin, Teacher, and Student roles.
- 📊 **Performance Monitoring**: Allow students to view their own academic progress and allow teachers/admins to manage results and exams.

---

## 👥 User Roles & Permissions

| Role     | Access Level                                                                 |
|----------|------------------------------------------------------------------------------|
| Admin    | Full access to all modules including managing users, roles, and data        |
| Teacher  | Access to their own profile, courses, assigned classes, exams, and grading  |
| Student  | View their personal data, enrolled courses, and exam results                |

---

## 🔄 Core Functionalities Per Module

Each major entity (Student, Teacher, Course, etc.) supports:
- Full **CRUD operations** (Create, Read, Update, Delete)
- Role-based access to related actions and views
- Detailed listings and filtered views
- Relational navigation (e.g., students in a class, exams in a course)

---

## 🧩 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Identity & Role Management
- Bootstrap (for responsive UI)
- LINQ for querying data
- Role-based Razor Layouts & Navigation

---

## 👨‍👩‍👧 Team
This system is developed by a team of 6 members.