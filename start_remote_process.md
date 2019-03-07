### ON REMOTE PC
Enable-PSRemoting -Force winrm quickconfig

winrm create winrm/config/Listener?Address=*+Transport=HTTP

winrm quickconfig
>> winrm set winrm/config/client '@{TrustedHosts="192.168.0.177"}'


### ON CLIENT PC

$pw = convertto-securestring -AsPlainText -Force -String yourpassword

$cred = new-object -typename System.Management.Automation.PSCredential -argumentlist "Kiki",$pw

$s = New-PSSession -ComputerName 192.168.0.166 -Credential $cred

Invoke-Command -Session $s -ScriptBlock {Get-Culture}

Copy-Item -ToSession $s -Path "C:\Users\vucko\Desktop\krneki.txt" -Destination "C:\Us
ers\Kiki\Desktop\krneki.txt" -Recurse

https://blogs.technet.microsoft.com/poshchap/2015/10/30/copy-to-or-from-a-powershell-session/
