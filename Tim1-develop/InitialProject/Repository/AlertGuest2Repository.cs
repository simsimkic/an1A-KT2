﻿using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class AlertGuest2Repository:IAlertGuest2Repository
    {
        private const string FilePath = "../../../Resources/Data/alertsGuest2.csv";

        private readonly Serializer<AlertGuest2> _serializer;

        private List<AlertGuest2> alerts;

        public AlertGuest2Repository()
        {
            _serializer = new Serializer<AlertGuest2>();
            alerts = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            alerts = _serializer.FromCSV(FilePath);
            if (alerts.Count < 1)
            {
                return 1;
            }
            return alerts.Max(c => c.Id) + 1;
        }
        public List<AlertGuest2> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public AlertGuest2 Save(AlertGuest2 alert)
        {
            alert.Id = NextId();
            alerts = _serializer.FromCSV(FilePath);
            alerts.Add(alert);
            _serializer.ToCSV(FilePath, alerts);
            return alert;
        }
        public AlertGuest2 Update(AlertGuest2 alert)
        {
            alerts = _serializer.FromCSV(FilePath);
            AlertGuest2 current = alerts.Find(c => c.Id == alert.Id);
            int index = alerts.IndexOf(current);
            alerts.Remove(current);
            alerts.Insert(index, alert);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, alerts);
            return alert;
        }

        public void Delete(AlertGuest2 alert)
        {
            alerts = _serializer.FromCSV(FilePath);
            AlertGuest2 founded = alerts.Find(c => c.Id == alert.Id);
            alerts.Remove(founded);
            _serializer.ToCSV(FilePath, alerts);
        }
        public List<AlertGuest2> GetByInstanceIdAndGuestId(int instanceId,int guestId)
        {
            alerts = _serializer.FromCSV(FilePath);
            List<AlertGuest2> lista= new List<AlertGuest2>();
            foreach(AlertGuest2 alert in alerts)
            {
                if(alert.InstanceId==instanceId && alert.Guest2Id == guestId)
                {
                    lista.Add(alert);
                }
            }
            return lista;
        }
        public AlertGuest2 GetById(int id)
        {
            return alerts.Find(c => c.Id == id);
        }
        public void AddAlerts(int currentPointId, int _callId,TourInstance selected)
        {      
            TourReservationService tourReservationService = new TourReservationService();
            List<TourReservation> availableReservations = tourReservationService.GetReservationsForTour(selected);
            foreach (TourReservation tour in availableReservations)
            {
                AlertGuest2 alertGuest2 = new AlertGuest2(tour.Id, tour.GuestId, currentPointId, selected.Id);
                AlertGuest2 savedAlert = Save(alertGuest2);
            }
        }

    }
}
