using System;

namespace MetX.Standard.Archived.Web
{
    
    /// <summary>The base class from which all Secured Web Forms should derive. Provides security and profile information as well as user page state and user profile state management in an ASP.NET Web Page.
    /// <para>By the time Page_Load is fired, the user's profile has been loaded, they have been authenticated, and have at least read permission on the page.</para>
    /// </summary>
    public class SecurePage : xlgPage
    {
        /// <summary>The SecureUserProfile containing the viewing user's security settings, profile, permissions, user profile state, and user page state.</summary>
        public SecureUserProfile Security = new SecureUserProfile();
        
        /// <summary>The override that callse Security.Start(). NOTE: Generally do NOT overload this method. If you do, make sure to call base.OnLoad(e) before doing anythig else.</summary>
        /// <param name="e">OnLoad event arguments passed from the Page object</param>
        protected override void OnLoad(EventArgs e)
        {
            Security.Start(Context);
            // Be sure to call the base class's OnLoad method!
            base.OnLoad(e);
        }
    }
} 
