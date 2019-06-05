using System.Diagnostics;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class TableDetailViewModel
    {
        public string name;
        private UserRepo userRepo;
        private RoomRepo roomRepo;
        private AppDatabase database;
        public TableDetailViewModel(string name, AppDatabase database)
        {
            this.name = name;
            this.database = database;
            userRepo = new UserRepo(database);
            roomRepo = new RoomRepo(database);
        }
        public Command DeleteRecordCommand
        {
            get
            {
                return new Command((id) =>
                {
                    switch (name)
                    {
                        case "SuperUser":
                            database.DatabaseNotAsync.Execute("DELETE FROM " + name + " WHERe Id=" + id);
                            break;
                        case "User":
                            if (database.Database.Table<User>().Where(i => i.Id == (int)id).FirstOrDefaultAsync().Result!=null)
                            {
                                var user = database.Database.Table<User>().Where(i => i.Id == (int)id).FirstOrDefaultAsync().Result;
                                if (roomRepo.GetRoomAsync(user.RoomNumber).Result != null)
                                {
                                    var room = roomRepo.GetRoomAsync(user.RoomNumber).Result;
                                    if (room.StudentA == user.Index)
                                    {
                                        room.StudentA = null;
                                        roomRepo.SaveRoomAsync(room);
                                        if(userRepo.GetUserAsync(room.StudentB).Result != null)
                                        {
                                            var roommate = userRepo.GetUserAsync(room.StudentB).Result;
                                            roommate.RoomMate = false;
                                            userRepo.SaveUserAsync(roommate);
                                        }
                                    }
                                    else if (room.StudentB == user.Index)
                                    {
                                        room.StudentB = null;
                                        roomRepo.SaveRoomAsync(room);
                                        if (userRepo.GetUserAsync(room.StudentA).Result != null)
                                        {
                                            var roommate = userRepo.GetUserAsync(room.StudentB).Result;
                                            roommate.RoomMate = false;
                                            userRepo.SaveUserAsync(roommate);
                                        }
                                    }
                                }
                                userRepo.DeleteUserAsync(user);
                            }
                            break;
                        case "Room":
                            if (database.Database.Table<Room>().Where(i => i.Id == (int)id).FirstOrDefaultAsync().Result != null)
                            {
                                var room = database.Database.Table<Room>().Where(i => i.Id == (int)id).FirstOrDefaultAsync().Result;
                                if (room.StudentA!=null)
                                {
                                    if (userRepo.GetUserAsync(room.StudentA).Result!=null)
                                    {
                                        var roommate = userRepo.GetUserAsync(room.StudentA).Result;
                                        roommate.RoomMate = false;
                                        roommate.RoomNumber = 0;
                                        userRepo.SaveUserAsync(roommate);
                                    }
                                }
                                if (room.StudentB!=null)
                                {
                                    if (userRepo.GetUserAsync(room.StudentB).Result != null)
                                    {
                                        var roommate = userRepo.GetUserAsync(room.StudentB).Result;
                                        roommate.RoomMate = false;
                                        roommate.RoomNumber = 0;
                                        userRepo.SaveUserAsync(roommate);
                                    }
                                }
                                roomRepo.DeleteRoomAsync(room);
                            }
                            break;
                        default:
                            Debug.WriteLine("ERROR, Unkown table");
                            break;
                    }
                    

                });
            }
        }
    }
}
