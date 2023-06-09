﻿using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class LocationRepository : ILocationRepository 
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();       
            _locations = _serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Location Save(Location location)
        {
            location.Id = NextId();
            _locations = _serializer.FromCSV(FilePath);
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
             return location;
        }

        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(c => c.Id) + 1;
        }

        public void Delete(Location location)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location founded = _locations.Find(c => c.Id == location.Id);
            _locations.Remove(founded);
            _serializer.ToCSV(FilePath, _locations);
        }

        public Location Update(Location location)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location current = _locations.Find(c => c.Id == location.Id);
            int index = _locations.IndexOf(current);
            _locations.Remove(current);
            _locations.Insert(index, location);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _locations);
            return location;
        }

        public int Add(Location location)
        {
            location.Id = NextId();
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
            return location.Id;
        }

        public ObservableCollection<string> GetCitiesByCountry(string country)
        {
            _locations=_serializer.FromCSV(FilePath);
            ObservableCollection<string> cities = new ObservableCollection<string>();
            foreach(Location location in _locations)
            {
                if(location.Country.Equals(country))
                {
                    if(!cities.Contains(location.City))
                    {
                        cities.Add(location.City);
                    }
                }
            }
            return cities;
        }

        public List<string> GetAllCountries()
        {
            _locations = _serializer.FromCSV(FilePath);
            List<string> countries = new List<string>();
            foreach(Location location in _locations)
            {
                if(!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }
            return countries;
        }

        public Location GetByCityAndCountry(string country, string city)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(n => n.City.Equals(city) && n.Country.Equals(country));
        }

        public Location GetById(int id) 
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(n => n.Id==id);
        }
    }
}
