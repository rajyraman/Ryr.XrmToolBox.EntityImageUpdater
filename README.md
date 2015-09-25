#What does this tool do?

CRM2015 introduced the [image attribute](https://msdn.microsoft.com/en-au/library/8597998f-764f-4c73-b63d-9f5e02c78061#BKMK_EntityImages).
This is quite useful for account and contact entity. You can use a company logo on the account record and a person's image
on the contact record. I envision the initial version of this tool to be used to store the company logo.

#How does it update the logo?
This tool uses the [Clearbit Logo API](https://clearbit.com/docs#logo-api) to find the logo based on the url.

#Install Instructions
Head the the Release area in Github and download the [latest release](https://github.com/rajyraman/Ryr.XrmToolBox.EntityImageUpdater/releases/latest). 
After download, right-click on the zip file, choose properties and click on Unprotect. The extract the zip file into the
Plugins folder of XrmToolBox.

#How does it look
![Entity Image Updater Tool Screenshot](http://imgur.com/zMQldqr?1)
