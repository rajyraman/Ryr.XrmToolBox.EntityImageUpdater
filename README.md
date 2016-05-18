#What does this tool do?

CRM2013 introduced the [image attribute](https://msdn.microsoft.com/en-au/library/8597998f-764f-4c73-b63d-9f5e02c78061#BKMK_EntityImages). This is quite useful for Account, Lead, and Contact entities. You can use this tool to retrieve images from the web based on Email, Url or Twitter handle. If you already have images in the correct sizes and names, you can use the tool to upload the images in the disk instead.

#How does it update the logo?

This tool uses the following APIs
* [Clearbit Logo API](https://clearbit.com/docs#logo-api) to find the logo based on the url
* Gravatar for email
* Twitter for a specified handle. The handle has to be specified with the "@"

#Install Instructions
Download the most recent version of XrmToolBox. Click on the "Plugin Store" button in the menu and install this tool from the store.

#Credits
I have re-used lot of Tanguy's code from the View Layout Replicator tool. I used this as the starting point, so that I can concentrate on the core functionality.

#How does it look
![](https://github.com/rajyraman/Ryr.XrmToolBox.EntityImageUpdater/blob/master/Screenshot.png)
