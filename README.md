# WinCouch
 
The one-click CouchDB package for Windows like [Jan Lehnardt's](https://github.com/janl) [CouchDBX](http://janl.github.com/couchdbx/) for Mac OS X.

### Based on

* [Apache CouchDB](http://couchdb.apache.org) - the open source NoSQL JSON database, running on [Erlang](http://erlang.org/)
* [GeckoFX](http://geckofx.org/)  - an open source component for embedding Mozilla Gecko (Firefox) in .Net applications
* We built it from source, so you don't have to

# Usage

1. Download and Unzip
2. Double-Click on WinCouch.exe

# TODO

* Add the source to the repository
* Add application configuration file


# Build from Source

This is not yet sufficient to build from source yourself - we're working on it. But at least you can see the sauce...

### WinCouch structure

    WinCouch\
        XULrunner\                       #embedded Gecko engine
        Skybound.Gecko.dll               #Gecko shim
        WinCouch.exe
        CouchDB\                         #CouchdB distro
            bin\erl.ini                  #erlang config
            erts-5.8.2\bin\erl.ini       #erlang config
            var\
                log\couchdb\couch.log    #logs
                lib\couchdb\             #database files
            etc\couchdb\local.ini        #customisable config

### Get the Bits

Download and unpack the following binaries into the above structure:

* [Apache CouchDB 1.0.2](https://github.com/downloads/dch/couchdb/setup-couchdb-1.0.2_otp_R14B01_spidermonkey_1.8.5.exe) binaries from [Dave Cottlehuber](https://github.com/dch) and [source](https://github.com/apache/couchdb/zipball/1.0.2)
* [GeckoFX](http://geckofx.org/)  [binaries](http://geckofx.googlecode.com/files/Skybound.GeckoFX.bin.v1.9.1.0.zip) and [source](http://geckofx.googlecode.com/files/Skybound.GeckoFX.src.v1.9.1.0.zip)
* Win32 OpenSSL from Win32 OpenSSL 1.0.0.c 
* set yourself a new environment variable, `%WinCouch%`

### Fiddle with the Bits

* convert `%wincouch%\couchdb\etc\couchdb\local.ini` to windows file format instead of unix
* copy both openssl DLLs into `%WinCouch%\couchdb\bin\`
* modify `couchdb.bat` to call `werl.exe` with `-detached` (so it doesn't show up in menu)
* modify both `erl.ini` files as follows:

        [erlang]
        Bindir=..\\erts-5.8.2\\bin
        Progname=erl
        Rootdir=..

* remove cruft (long list)

        appmon-2.1.13
        asn1-1.6.15
        common_test-1.5.2
        compiler-4.7.2
        cosEvent-2.1.9
        cosEventDomain-1.1.9
        cosFileTransfer-1.1.10
        cosNotification-1.1.15
        cosProperty-1.1.12
        cosTime-1.1.9
        cosTransactions-1.2.10
        debugger-3.2.5
        dialyzer-2.4.0
        docbuilder-0.9.8.9
        edoc-0.7.6.8
        erl_docgen-0.2.3
        erl_interface-3.7.2
        et-1.4.2
        etap
        eunit-2.1.6
        gs-1.5.13
        hipe-3.7.8
        ic-4.2.25
        inviso-0.6.2
        megaco-3.15
        mnesia-4.4.16
        observer-0.9.8.4
        odbc-2.10.9
        orber-3.6.18
        os_mon-2.2.5
        otp_mibs-1.0.6
        parsetools-2.0.5
        percept-0.8.4
        pman-2.7.1
        snmp-4.18
        syntax_tools-1.6.7
        test_server-3.4.2
        toolbar-1.4.1
        tools-2.6.6.2
        tv-2.1.4.6
        typer-0.1.7.5
        webtool-0.8.7
        wx-0.98.8
        
* or more quickly

        pushd %WinCouch%\couchdb\lib && rd /s/q appmon-* asn1-* common_test-* compiler-* cosEvent-* cosEventDomain-* cosFileTransfer-* cosNotification-* cosProperty-* cosTime-* cosTransactions-* debugger-* dialyzer-* docbuilder-* edoc-* erl_docgen-* erl_interface-* et-* etap eunit-* gs-* hipe-* ic-* inviso-* megaco-* mnesia-* observer-* odbc-* orber-* os_mon-* otp_mibs-* parsetools-* percept-* pman-* snmp-* syntax_tools-* test_server-* toolbar-* tools-* tv-* typer-* webtool-* wx-*

* put some WinCouch & Gecko bits in here next
