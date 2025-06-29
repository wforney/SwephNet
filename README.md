SwephNet
===========
This project is an Astrodienst Swiss Ephemeris (http://www.astro.com/swisseph/) .Net version in a 
PCL project for cross platform usage.

It is a rewrite of the (https://github.com/ygrenier/SwissEphNet) project with the .Net guidelines, 
so it's an .Net optimization of the SwissEphNet project but with a different interface.

2025.06.29: Update - Added a new project to upgrade .NET 9 as well as updating to .NET Framework 4.8 on the older projects.

The two projects will continue to exist in parallel :
- SwissEphNet : is the direct C to C# portage of the Swiss Ephemeris.
- SwephNet : is the full .Net implementation of the Swiss Ephemeris.
- Sweph.Net : is the new .NET 9 implementation which is a refactor of SwephNet without the old C style patterns.

This project is currently under development, so you can't use it prefer 
(https://github.com/ygrenier/SwissEphNet) instead.

## Status

Current version : 2.0.0

[![Build status](https://ci.appveyor.com/api/projects/status/calggagl2eoxjnyu)](https://ci.appveyor.com/project/ygrenier/swephnet)
