README of asminfo
=================

* Copyright (C) 2012 Adam Brengesjö
* This application is licensed under [GNU General Public License, Version 3.0]


Summary
-------

Prints information about a .NET assembly.


About
-----

I use Git for most of my work, even to version a binary installation of
a product, in combination with [git-ftp], I have been using Git
with great success to deploy to QA and production enviornments.

But I could not get the assembly version of a assembly for a specific
commit without checkout out the file and open the properties for that
file.

So this program, in combination with the text converter feature of
git-diff, this tool helps me quickly preview to what version
a binary file was upgraded/downgraded to.


Known issues and limitations
----------------------------

* See [asminfo issues on GitHub] for open issues

* When performing git-diff on a binary which which content have
  changed, but the assembly information is the same, git-diff
  will still get the identical output from asminfo.exe, thus
  omitting the file in lists.

* asminfo.exe does not work with native assemblies, 
  only .NET assemblies.


Installing
----------

These instructions assume you use the Git Bash terminal in Windows.

	$ ./build.sh
	$ ./install.sh

This will install asminfo.exe in /bin/ (identical to 
C:\Program Files (x86)\Git\bin or simlar).


Howto configure Git to use asminfo.exe
--------------------------------------

This is an example of how to configure to let Git use asminfo.exe

	$ echo "*.exe diff=dnetasm" >>~/.gitattributes
	$ echo "*.dll diff=dnetasm" >>~/.gitattributes
	$ git config --global core.attributesfile ~/.gitattributes
	$ git config --global diff.dnetasm.textconv asminfo


Contributions
-------------

Have a cool feature? Want to improve something? Want to change something?
Be social! Use GitHub :)


[git-ftp]: http://github.com/resmo/git-ftp
[asminfo issues on GitHub]: http://github.com/adbre/asminfo/issues
[GNU General Public License, Version 3.0]: http://www.gnu.org/licenses/gpl-3.0-standalone.html
