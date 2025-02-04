# Azure Storage Management with .NET MVC

This repository demonstrates how to interact with Azure Storage services using the Azure Storage SDK within a .NET MVC web application.  It provides functionalities for managing Storage Queues, Tables, and Blobs, showcasing practical cloud development skills.

## Table of Contents

* [Project Overview](#project-overview)
* [Technical Stack](#technical-stack)
* [Architecture](#architecture)
* [Key Features](#key-features)
* [Azure Storage Interactions](#azure-storage-interactions)
* [Future Enhancements](#future-enhancements)

## Project Overview

This project provides a hands-on example of how to leverage Azure Storage services within a .NET MVC web application. Users can perform CRUD operations on Azure Storage entities, demonstrating essential cloud development skills. The application covers managing Storage Queues, Tables, and Blobs, providing a comprehensive overview of Azure Storage capabilities.

## Technical Stack

* **.NET:** .NET 8
* **ASP.NET MVC:** Implementation of the Model-View-Controller design pattern.
* **Azure Storage SDK:**  Library for interacting with Azure Storage services.
* **Semantic Kernel:** (1.0.0-beta3) - For integrating chat assistant capabilities.

## Architecture

The project follows a standard MVC architecture and two layers for storage processes. 

* **Models in Storage Library Layer:** Contains the data models representing the entities stored in Azure Storage (e.g., Queue messages, Table entities, Blob metadata).
* **Views:** Handles the user interface and presentation logic.
* **Controllers:** Manages user interactions and interacts with the Azure Storage SDK to perform operations on Storage Queues, Tables, and Blobs.

## Key Features

* **Azure Storage Queue Management:** Create, delete, and manage Azure Storage Queues.
* **Azure Storage Table Operations:** Perform CRUD operations on Azure Storage Tables (Create, Read, Update, Delete).
* **Azure Storage Blob Management:** Upload, download, and manage Azure Storage Blobs (including listing blobs, setting metadata, etc.).
* **Uses Azure Storage SDK:** Demonstrates proper usage of the Azure Storage SDK for .NET.
* **Asynchronous Operations:** Asynchronous methods for improved performance.
* **Semantic Kernel Integration:**  Integrates Semantic Kernel for chat assistant functionality (This is for integration of AI capabilities to a project).

## Azure Storage Interactions

The project demonstrates how to perform the following actions with Azure Storage:

* **Queues:** Creating queues, adding messages, retrieving messages, deleting messages, deleting queues.
* **Tables:** Creating tables, inserting entities, retrieving entities, updating entities, deleting entities, querying tables.
* **Blobs:** Uploading blobs, downloading blobs, listing blobs, deleting blobs, setting metadata, getting metadata.

## Future Enhancements

* **Error Handling:** Implement robust error handling and logging.
* **Security Best Practices:** Incorporate security best practices for accessing Azure Storage.
* **Unit Tests:** Add unit tests to ensure code quality and correctness.
* **Configuration Management:** Implement proper configuration management for Azure Storage connection strings.