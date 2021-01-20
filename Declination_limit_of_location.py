#!/usr/bin/env python
# coding: utf-8

# # Usage description
# Some targets may never been seen from a certain location. For example, when you're at North Pole, you can only see the stars in the northern hemisphere. 
# However, if you are standing on the Equator, you can see the whole sky. Due to this reason, 
# we don't want to recommend users to join projects which they can't even observe the targets in them. 
# Therefore, use this simple script to calculate the target declination limit of a certain location.  
# 
# ### The columns you need for the databases are:  
# **U_have_E (User's equipment)**
# - longitude (deg)
# - latitude (deg)
# - altitude (m)
# - elevation limit of equipment (deg)
# 
# ### The returned value is:
# - declination lower limit (low_dec)
# 
# If low_dec < 0, the observable declination range is +90 ~ low_dec.  
# If low_dec > 0, the observable declination range is low_dec ~ -90.

# In[4]:


from astropy.coordinates import SkyCoord, EarthLocation, Angle
import astropy.units as u
from astropy.time import Time


# In[5]:


def declination_limit(longitude, latitude, altitude, elevation_limit):
    time = Time('2020-01-01T20:00:00')
    # Time does not need to be changed. Declination limit doesn't change with time.
    site_inf = EarthLocation(lon = longitude*u.deg, lat = latitude*u.deg, height = altitude*u.m)
    if latitude >= 0:
        target = SkyCoord(alt = elevation_limit*u.deg, az = 180.0*u.deg, frame = 'altaz', obstime = time, location = site_inf)
    else:
        target = SkyCoord(alt = elevation_limit*u.deg, az = 0.0*u.deg, frame = 'altaz', obstime = time, location = site_inf)
    low_dec = target.icrs.dec.degree
    return low_dec


# In[6]:

import sys

# Given arguments. Import this from your databases!!!

# Equipment information
# longitude = 120
# latitude = -24
# altitude = 50
# elevation_limit = 10
longitude = float(sys.argv[1])
latitude = float(sys.argv[2])
altitude = float(sys.argv[3])
elevation_limit = float(sys.argv[4])


# In[7]:


if __name__ == '__main__':
    low_dec = declination_limit(longitude, latitude, altitude, elevation_limit)
    print(low_dec)


# In[ ]:




