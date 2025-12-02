#!/bin/bash

echo ">>> Updating system"
sudo apt-get update -y

echo ">>> Installing Node.js 20"
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

echo ">>> Installing .NET 8 SDK + ASP.NET Runtime"
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update -y
sudo apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0

###########################################################################################
#              SQL Server installation FIRST (before any dotnet apps start)              #
###########################################################################################

echo ">>> Installing SQL Server 2022"
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list)"
sudo apt-get update -y
sudo ACCEPT_EULA=Y apt-get install -y mssql-server

echo ">>> Configuring SQL Server"
sudo MSSQL_SA_PASSWORD="YourStrong!Passw0rd" MSSQL_PID="Express" /opt/mssql/bin/mssql-conf -n setup

echo ">>> Installing SQL tools"
sudo ACCEPT_EULA=Y apt-get install -y msodbcsql17
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc

# -------- RESTORE PRIVATBANKDB IF EXISTS ----------
if [ -f "/backup/PrivatBankDb.bak" ]; then
    echo ">>> Restoring PrivatBankDb from backup"
    sqlcmd -S localhost,1433 -U sa -P YourStrong!Passw0rd -Q "RESTORE DATABASE PrivatBankDb FROM DISK='/backup/PrivatBankDb.bak' WITH REPLACE;"
else
    echo ">>> No backup found, skipping restore"
fi

###########################################################################################
#                     AFTER SQL IS READY, install client + servers                      #
###########################################################################################

echo ">>> Preparing React client folder"
rm -rf /home/vagrant/app-client
cp -r /client-shared /home/vagrant/app-client

echo ">>> Installing React dependencies"
cd /home/vagrant/app-client
npm install

echo ">>> Starting ASP.NET API"
cd /server
dotnet restore
nohup dotnet run --urls "http://0.0.0.0:5209" > /home/vagrant/api.log 2>&1 &

echo ">>> Starting AuthServer"
cd /auth
dotnet restore
nohup dotnet run --urls "http://0.0.0.0:5001" > /home/vagrant/auth.log 2>&1 &

echo ">>> Starting React Client"
cd /home/vagrant/app-client
nohup npm run dev -- --host > /home/vagrant/react.log 2>&1 &

echo ">>> ALL services started"
