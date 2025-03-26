// Add these lines to your Program.cs file in the appropriate locations

// Configure to serve the frontend SPA
app.UseDefaultFiles();
app.UseStaticFiles();

// This should be placed last, after all API routes, to handle all other requests with the SPA
app.MapFallbackToFile("index.html");
