#What does this tool do?

CRM2015 introduced the [image attribute](https://msdn.microsoft.com/en-au/library/8597998f-764f-4c73-b63d-9f5e02c78061#BKMK_EntityImages).
This is quite useful for account and contact entity. You can use a company logo on the account record and a person's image on the contact record. I envision the initial version of this tool to be used to store the company logo. In
the future versions of the tool, I plan to upload images from disk into a contact or any other entity with the image
field based on the primary key or a set matching criteria.

#How does it update the logo?
This tool uses the [Clearbit Logo API](https://clearbit.com/docs#logo-api) to find the logo based on the url.

#Install Instructions
Head to the the Release area in Github and download the [latest release](https://github.com/rajyraman/Ryr.XrmToolBox.EntityImageUpdater/releases/latest). 
After download, right-click on the zip file, choose properties and click on Unprotect. Extract the zip file into the
Plugins folder of XrmToolBox.

#Credits
I have re-used lot of Tanguy's code from the View Layout Replicator tool. I used this as the starting point, so that I can concentrate on the core functionality.

#How does it look
![](https://github.com/rajyraman/Ryr.XrmToolBox.EntityImageUpdater/blob/master/EntityImageUpdater.png)
