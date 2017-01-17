# no-username-telnet-bruter
Telnet bruter for systems with no username like some video conferencing devices including anti-bruteforce-detection-and-mitigation

Herpa derp derp derp.
We got tasked with auditing the video conferencing infrastructure (cisco devices - i forget the models, will come update this) and tools like hydra and brutus etc couldn't do the job.
False positives for every ~3rd password attempt because the telnet daemon on these devices are modified to dissuade bruteforce login tools like beforementioned.

So I got this little silly telnet client from codeproject and modified it to deal with these funky daemons and still get some bruteforcing love.

