![Platform](https://img.shields.io/badge/ASP.NET%20Core-6.0-brightgreen)

# WebAds

Architecture: Layered Architecture with MVC

Implemented:
  <ul>
  <li type='1'>Authorization and Registration</li>
   <ul>
    <li>Google & Facebook</li>
    <li>2FA (Multi-factor authentication)</li> 
   </ul>
   
  <li type='1'>User</li>
   <ul>
    <li>Profile editor.</li>
    <li>Abiliy to create custom ads and edit them.</li> 
    <li>Abiliy to add comments</li>
   </ul>
    <li type='1'>Manager (Has the right to control 'User' ads)</li>
    <li type='1'>Swagger</li>
    <li type='1'>Logging (Azure)</li>
  </ul>
  
  
Libraries:
  <ul>
    <li type='1'>Microsoft.AspNetCore.Authentication.Facebook</li>
    <li type='1'>Microsoft.AspNetCore.Authentication.Google</li>
    <li type='1'>Microsoft.AspNetCore.Identity</li>
    <li type='1'>Microsoft.EntityFrameworkCore.</li>
    <li type='1'>Serilog</li>
    <li type='1'>...</li>
  </ul>
    
