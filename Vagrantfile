# ---------------------------------------------------------
# VAGRANTFILE FOR .NET + POSTGRES + REACT
# ---------------------------------------------------------

Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"
  config.vm.hostname = "election-vm"

  config.vm.network "forwarded_port", guest: 5209, host: 5209
  config.vm.network "forwarded_port", guest: 5001, host: 5001
  config.vm.network "forwarded_port", guest: 5173, host: 5173

  # ---- SYNC ONLY SOURCE CODE (do NOT run from these folders!) ----
  config.vm.synced_folder "./ElectionAppReact.Server", "/server-shared", create: true
  config.vm.synced_folder "./AuthServer", "/auth-shared", create: true
  config.vm.synced_folder "./electionappreact.client", "/client-shared", create: true


  # ---- PROVISION ----
  config.vm.provision "shell", path: "./provision.sh"
end
