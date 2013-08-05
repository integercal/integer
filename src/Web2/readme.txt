If IIS Express is running stop it
Open your webapplication project file (*.csproj or *.vbproj)
Find <IISUrl>http://localhost:61156/</IISUrl> and change it to <IISUrl>http://devserver.com:61156/</IISUrl>
Open %userprofile%\documents\iisexpress\config\applicationhost.config file
Find your site entry in applicationhost.config file change the binding as shown below
<binding protocol="http" bindingInformation="*:61156:devserver.com" />
In \Windows\system32\drivers\etc\hosts file add following mapping "127.0.0.1 devserver.com"
In your browser add exception to bypass proxy for devserver.com
Note that, since you are using custom domain (non localhost binding), you must run visual studio as Administrator