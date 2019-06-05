using System.Collections.Generic;
using System.Linq;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class MatchViewModel : BaseViewModel
    {
        private const int maxPoints = 13;
        public readonly string index;
        private readonly RoomRepo roomRepo;
        private readonly User user;
        private readonly UserRepo userRepo;
        private AppDatabase database;
        public List<Room> Rooms;
        public List<User> Users;
        private string bestCandidate = "";
        private int points;

        public MatchViewModel(string index, AppDatabase database)
        {
            this.index = index;
            this.database = database;
            userRepo = new UserRepo(database);
            roomRepo = new RoomRepo(database);
            Rooms = roomRepo.GetRoomsAsync().Result;
            Users = userRepo.GetUsersAsync().Result;
            user = userRepo.GetUserAsync(index).Result;
        }

        public void SavePoints()
        {
            points = 0;
            foreach (var usr in Users)
                if (usr.Index != index && usr.Sex == user.Sex && usr.RoomMate == false)
                {
                    if (points == maxPoints) break;
                    CountPoints(usr);
                }
        }

        public void CountPoints(User user)
        {
            var points = 0;
            if (user.Floor == this.user.Floor) points++;
            if (user.BedLocation != this.user.BedLocation) points++;
            if (user.SleepTime == this.user.SleepTime) points++;
            if (user.WakeUpTime == this.user.WakeUpTime) points++;
            if (user.HotOrNot == this.user.HotOrNot) points++;
            if (user.Music == this.user.Music) points++;
            if (user.CleanUp == this.user.CleanUp) points++;
            if (user.Talkative == this.user.Talkative) points++;
            if (user.StudyField == this.user.StudyField) points++;
            if (user.Sporting == this.user.Sporting) points++;
            if (user.HomeBack == this.user.HomeBack) points++;
            if (user.Smoking == this.user.Smoking) points++;
            if (user.Party == this.user.Party) points++;

            if (points > this.points)
            {
                this.points = points;
                bestCandidate = user.Index;
            }
        }

        public Room FindFreeRoom()
        {
            IEnumerable<Room> rooms = Rooms.Where(r => r.StudentA == null && r.StudentB == null);
            foreach (var room in rooms)
            {
                if (room.Floor != user.Floor)
                    continue;
                else return room;
            }
            return rooms.Any() ? rooms.First() : null;
        }

        public static bool RoomHasFreeSlot(Room room)
        {
            if (room.StudentA == null || room.StudentB == null)
                return true;
            return false;
        }

        public void ContactWithAdmin()
        {
            Wynik = "Coś poszło nie tak. Skontaktuj się z adminem.";
        }

        public void Accomodate(Room room)
        {
            var user = userRepo.GetUserAsync(bestCandidate).Result;
            if (room.StudentA == null) room.StudentA = this.user.Index;
            else room.StudentB = this.user.Index;
            user.RoomMate = true;
            this.user.RoomMate = true;
            this.user.RoomNumber = room.RoomNumber;
            roomRepo.SaveRoomAsync(room);
            userRepo.SaveUserAsync(user);
            userRepo.SaveUserAsync(this.user);
            Wynik = $"Gratulacje, Ty i Twój współlokator pasujecie do siebie w {(points * 100) / maxPoints} procentach! Twój pokój to {room.RoomNumber}";
        }
        
        public void DecideWhatToDo()
        {
            SavePoints();
            User user = null;
            if (points >= 10)
            {
                user = userRepo.GetUserAsync(bestCandidate).Result;
                var room = roomRepo.GetRoomAsync(user.RoomNumber).Result;
                if (!RoomHasFreeSlot(room))
                    ContactWithAdmin();
                else Accomodate(room);
            }
            else
            {
                var room = FindFreeRoom();
                if (room == null)
                {
                    var roomBestCandidate = roomRepo.GetRoomAsync(user.RoomNumber).Result;
                    if (!RoomHasFreeSlot(roomBestCandidate))
                        ContactWithAdmin();
                    else Accomodate(roomBestCandidate); 
                }
                else
                {
                    room.StudentA = this.user.Index;
                    this.user.RoomNumber = room.RoomNumber;
                    roomRepo.SaveRoomAsync(room);
                    userRepo.SaveUserAsync(this.user);
                    Wynik = $"Twój pokój to {room.RoomNumber}.";
                }
            }
        }
    }
}