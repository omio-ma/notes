# Auth0 Setup Guide for Notes Application

This guide will walk you through setting up Auth0 authentication for your Notes React application.

## Prerequisites

- An Auth0 account (sign up at https://auth0.com if you don't have one)
- Your React application running locally on `https://localhost:3000`

## Step 1: Create an Auth0 Account

1. Go to https://auth0.com
2. Click "Sign Up" and create a free account
3. Choose a tenant domain (e.g., `your-app-name.auth0.com`)
   - **Important**: Remember this domain - you'll need it later!

## Step 2: Create a New Application in Auth0

1. Log in to your Auth0 Dashboard: https://manage.auth0.com
2. Navigate to **Applications** → **Applications** in the left sidebar
3. Click the **"+ Create Application"** button
4. Configure your application:
   - **Name**: Enter a name (e.g., "Notes App")
   - **Application Type**: Select **"Single Page Web Applications"**
   - Click **"Create"**

## Step 3: Configure Application Settings

After creating your application, you'll be taken to the application settings page.

### 3.1 Note Your Application Credentials

In the **"Settings"** tab, you'll find:
- **Domain**: (e.g., `your-tenant.auth0.com`)
- **Client ID**: (e.g., `abc123xyz456...`)

**Copy these values** - you'll need them for your `.env` file!

### 3.2 Configure Application URIs

Scroll down to the **"Application URIs"** section and configure:

#### Allowed Callback URLs
```
https://localhost:3000
```

#### Allowed Logout URLs
```
https://localhost:3000
```

#### Allowed Web Origins
```
https://localhost:3000
```

#### Allowed Origins (CORS)
```
https://localhost:3000
```

### 3.3 Save Changes

Scroll to the bottom and click **"Save Changes"**

## Step 4: Create an API (for Backend Integration)

This step is needed if you want Auth0 to issue access tokens for your backend API.

1. Navigate to **Applications** → **APIs** in the left sidebar
2. Click **"+ Create API"**
3. Configure your API:
   - **Name**: `Notes API`
   - **Identifier**: `https://notes-api` (this is your API audience)
     - **Important**: This can be any URL-like identifier - it doesn't need to be a real URL
     - Example: `https://notes-api`, `https://api.notes.com`, or `https://your-domain.com/api`
   - **Signing Algorithm**: `RS256`
4. Click **"Create"**

**Copy the API Identifier** - you'll need this for your `.env` file as `VITE_AUTH0_AUDIENCE`!

## Step 5: Update Your React App Environment Variables

1. Open the `/client/.env` file in your project
2. Update it with your Auth0 credentials:

```env
# Auth0 Configuration
VITE_AUTH0_DOMAIN=your-tenant.auth0.com
VITE_AUTH0_CLIENT_ID=your-client-id-here
VITE_AUTH0_CALLBACK_URL=https://localhost:3000
VITE_AUTH0_AUDIENCE=https://notes-api

# API Configuration
VITE_API_URL=http://localhost:5145
```

### Where to find each value:

- **VITE_AUTH0_DOMAIN**: From Step 3.1 (Application Settings → Domain)
- **VITE_AUTH0_CLIENT_ID**: From Step 3.1 (Application Settings → Client ID)
- **VITE_AUTH0_CALLBACK_URL**: Your app's URL (https://localhost:3000)
- **VITE_AUTH0_AUDIENCE**: From Step 4 (API → Identifier)
- **VITE_API_URL**: Your backend API URL

## Step 6: Test Your Setup

1. Make sure your environment variables are saved
2. Restart your React development server:
   ```bash
   cd client
   npm run dev
   ```
3. Open your browser to `https://localhost:3000`
4. You should be redirected to the Auth0 login page
5. Try creating a new account or logging in

## Step 7: Optional - Configure Social Connections

If you want to allow users to log in with Google, GitHub, etc.:

1. Navigate to **Authentication** → **Social** in the Auth0 Dashboard
2. Click on the provider you want to enable (e.g., Google, GitHub)
3. Follow the provider-specific setup instructions
4. Enable the connection for your application

### Example: Google Social Login

1. Click on **Google** in the Social connections list
2. You can use Auth0's development keys for testing, or:
   - Create a Google OAuth 2.0 Client ID in Google Cloud Console
   - Add the Client ID and Secret to Auth0
3. In the **Applications** tab, enable your "Notes App"
4. Save changes

## Step 8: Configure Your Backend API (Optional)

If you're using the .NET backend, you'll need to configure it to validate Auth0 JWT tokens:

1. Install the required NuGet package:
   ```bash
   dotnet add src/Notes.API/Notes.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
   ```

2. Update your `Program.cs` to add JWT authentication:
   ```csharp
   using Microsoft.AspNetCore.Authentication.JwtBearer;

   builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.Authority = "https://your-tenant.auth0.com/";
           options.Audience = "https://notes-api";
       });

   app.UseAuthentication();
   app.UseAuthorization();
   ```

3. Add `[Authorize]` attribute to your controllers:
   ```csharp
   [ApiController]
   [Route("[controller]")]
   [Authorize]
   public class NotesController : ControllerBase
   {
       // Your endpoints
   }
   ```

## Troubleshooting

### Issue: "Invalid Callback URL"
- Make sure `https://localhost:3000` is added to **Allowed Callback URLs** in Auth0
- Ensure there are no trailing slashes

### Issue: "Access token does not contain a valid audience"
- Verify that `VITE_AUTH0_AUDIENCE` in `.env` matches the API Identifier in Auth0
- Make sure you created an API in Step 4

### Issue: "Login works but API calls fail with 401"
- Check that the backend is configured to validate Auth0 tokens (Step 8)
- Verify the `audience` in your backend matches the Auth0 API Identifier
- Check browser console for token errors

### Issue: "Cannot connect to https://localhost:3000"
- Make sure your React app is running with HTTPS
- The vite config includes `mkcert` plugin for HTTPS support
- If needed, regenerate SSL certificates

### Issue: CORS errors
- Add `https://localhost:3000` to **Allowed Origins (CORS)** in Auth0
- Check your backend CORS configuration allows your frontend origin

## Testing Checklist

- [ ] Can access the application and get redirected to Auth0 login
- [ ] Can sign up with email/password
- [ ] Can log in with existing credentials
- [ ] User profile displays correctly after login
- [ ] Can log out successfully
- [ ] Notes API calls include the Bearer token (check Network tab)
- [ ] Protected routes redirect to login when not authenticated

## Next Steps

1. **Customize the Login Page**:
   - Go to **Branding** → **Universal Login** in Auth0 Dashboard
   - Customize colors, logo, and styling

2. **Add User Metadata**:
   - Store additional user information in Auth0
   - Access it via `user.user_metadata`

3. **Set Up Email Templates**:
   - Customize welcome emails, password reset emails
   - Go to **Branding** → **Email Templates**

4. **Configure Multi-Factor Authentication (MFA)**:
   - Add extra security with MFA
   - Go to **Security** → **Multi-factor Auth**

5. **Monitor Your Application**:
   - View logs and analytics
   - Go to **Monitoring** → **Logs**

## Resources

- [Auth0 React Quickstart](https://auth0.com/docs/quickstart/spa/react)
- [Auth0 React SDK Documentation](https://github.com/auth0/auth0-react)
- [Auth0 Dashboard](https://manage.auth0.com)
- [Auth0 Community](https://community.auth0.com/)

## Support

If you encounter issues:
1. Check the Auth0 logs: Dashboard → Monitoring → Logs
2. Check browser console for errors
3. Verify all environment variables are correct
4. Ensure URLs match exactly (including https://)

---

**Congratulations!** 🎉 Your Auth0 authentication is now set up!
