#!/bin/bash

set -e

echo ">>> Updating system"
sudo apt-get update -y

echo ">>> Installing Node.js 20"
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

echo ">>> Installing .NET 8 SDK"
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update -y
sudo apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0

echo ">>> Installing PostgreSQL"
sudo apt-get install -y postgresql postgresql-contrib

echo ">>> Configuring PostgreSQL"
sudo -u postgres psql -c "ALTER USER postgres WITH PASSWORD '1234';"
sudo -u postgres createdb monobankdb || true
sudo -u postgres createdb electionsdb || true

echo ">>> Copying backend projects"
rm -rf /home/vagrant/app-server
rm -rf /home/vagrant/app-auth
rm -rf /home/vagrant/app-client

cp -r /server-shared /home/vagrant/app-server
cp -r /auth-shared /home/vagrant/app-auth
cp -r /client-shared /home/vagrant/app-client

sudo chown -R vagrant:vagrant /home/vagrant/app-server
sudo chown -R vagrant:vagrant /home/vagrant/app-auth
sudo chown -R vagrant:vagrant /home/vagrant/app-client

echo ">>> Adding Linux-only config"
cat <<EOF > /home/vagrant/app-server/appsettings.Development.json
{
  "RunMode": "Linux",
  "ConnectionStrings": {
    "MonobankPostgres": "Host=localhost;Database=monobankdb;Username=postgres;Password=1234",
    "ElectionPostgres": "Host=localhost;Database=electionsdb;Username=postgres;Password=1234"
  }
}
EOF

echo ">>> Restoring .NET"
cd /home/vagrant/app-server
dotnet restore

cd /home/vagrant/app-auth
dotnet restore

echo ">>> Installing React deps"
cd /home/vagrant/app-client
npm install

echo ">>> Starting servers"
cd /home/vagrant/app-server
nohup dotnet run --urls "http://0.0.0.0:5209" > /home/vagrant/api.log 2>&1 &

cd /home/vagrant/app-auth
nohup dotnet run --urls "http://0.0.0.0:5001" > /home/vagrant/auth.log 2>&1 &

cd /home/vagrant/app-client
nohup npm run dev -- --host > /home/vagrant/react.log 2>&1 &

echo ">>> ALL SERVICES ARE RUNNING!"
