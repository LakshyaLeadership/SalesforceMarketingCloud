# SFMC Email Marketing Migration

This repository contains code and instructions for migrating an automated email marketing process from Eloqua to Salesforce Marketing Cloud (SFMC). The migration involves pulling daily email content from a CMS via APIs and using SFMC APIs to create and send emails to a subscriber base of approximately 200k.

## Overview

The migration process consists of the following steps:

1. **Pull Email Content from CMS**: A C# program is implemented to retrieve daily email content (title, text, images, etc.) from a CMS through its API endpoints.

2. **Create Email in SFMC**: The retrieved email content is utilized to create an email template in SFMC using SFMC API integration. The email template is customizable and supports dynamic content insertion.

3. **Add Subscribers to SFMC List**: The program interacts with SFMC's API to add subscribers to the target subscriber list in SFMC. Subscriber properties such as email address, first name, last name, and other relevant details can be included.

4. **Schedule Email Send in SFMC**: The program schedules the email send in SFMC using SFMC API integration. The email send can be automated to occur daily at 3 am, ensuring timely delivery to the subscriber base.

## Prerequisites

To successfully execute the migration process, the following prerequisites are required:

- Eloqua API access and authentication credentials.
- SFMC API access and authentication credentials.
- Access to the CMS API and its endpoint URL.
- SFMC subscriber list ID for adding subscribers.
- SFMC email template ID for creating emails.

## Setup and Execution

Clone the repository to your local machine:

   ```shell
   git clone https://github.com/your-username/sfmc-email-migration.git


Update the following configuration variables in the C# code:

cmsApiUrl: Replace with the URL of your CMS API endpoint.
sfmcApiUrl: Replace with the URL of your SFMC API endpoint.
sfmcSubscriberListId: Replace with the ID of your target subscriber list in SFMC.
sfmcEmailTemplateId: Replace with the ID of your email template in SFMC.
Install the necessary dependencies (if any) required by the C# program.

Compile and run the C# program.

Monitor the program execution for any errors or exceptions. Make sure to handle errors appropriately and implement exception management as needed.

Additional Steps
To enhance the migration process and further customize the solution, you may consider implementing the following:

Retrieve an access token for SFMC API authentication: Modify the code to retrieve an access token using the client ID and client secret. This ensures secure authentication when making API calls to SFMC.

Error Handling and Exception Management: Extend the code with proper error handling and exception management to handle potential issues during the API calls and program execution.

Logging and Monitoring: Implement a logging mechanism to capture relevant information and monitor the execution of the program. This helps in troubleshooting and ensuring the smooth functioning of the migration process.
