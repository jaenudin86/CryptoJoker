# CryptoJoker

[![Build status](https://ci.appveyor.com/api/projects/status/9vq5wxdty0wtfh8p?svg=true)](https://ci.appveyor.com/project/xXxRevolutionxXx/cryptojoker)

## Description

CryptoJoker is a managed ransomware created for EDUCATION PURPOSES ONLY written exclusively in C#. Even though CryptoJoker has been succesfully tested
on a Windows 10 Virtual Machine, is not meant to work in real world. CryptoJoker has been coded with safety and fastness in mind without "burden" one another.
CryptoJoker uses a combination of a "custom XOR" encryption and RSA. A private public/private pair key is generated for every computer ... and the private
key is delivered back to the "owner" through an e-mail account. The delivery method can very easily change to FTP or whatever you want.

## How to use

If you make a change in the infector you just have to re-build the project. If you make a change in the decryptor after building it, you need to replace it with the old one in the infector's resources.

## Dependency

The dependency of CryptoJoker(both infector and decryptor) is .NET framework 2.0 ... which is the smallest possible.

## Script Kiddies

This project even though is compile ready isn't meant for script kiddies that want to make some bucks by infecting people. 
The infector has been uploaded to Virus Total and is detected.

## Support

You can support this project by either reporting bugs or by writing code. Both are welcome !!
