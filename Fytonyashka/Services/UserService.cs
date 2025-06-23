using System.Text.Json;
using Fytonyashka.DTOs;

namespace Fytonyashka.Services
{
    public interface IUserService
    {
        bool Create(UserDto userDto);
        bool Login(string username, string password);
        void Logout(string username);
        List<UserDto> GetAll();
        bool Delete(int id);
        UserDto GetById(int id);
        bool Update(UserDto userDto);
        UserDto GetByUsername(string username);
        void RemoveAvatar(int id);
        void UpdateAvatar(int id, string avatarFileName);
    }

    internal class UserService : IUserService
    {
        private int _nextId = 1;
        private readonly string _dataFilePath;
        private List<UserDto> _users = new();
        private List<string> LoggedUserNames { get; set; } = new List<string>();

        public UserService() {
            string baseDirectory = Directory.GetCurrentDirectory();
            string dataDirectory = Path.Combine(baseDirectory, "Data");
            if (!Directory.Exists(dataDirectory)) {
                Directory.CreateDirectory(dataDirectory);
            }
            _dataFilePath = Path.Combine(dataDirectory, "users.json");

            if (File.Exists(_dataFilePath)) {
                Load();
            }
        }

        private void Load() {
            string userJsons = File.ReadAllText(_dataFilePath);
            _users = JsonSerializer.Deserialize<List<UserDto>>(userJsons) ?? new List<UserDto>();
            InitializeNextId();
        }

        private void InitializeNextId() {
            foreach (UserDto user in _users) {
                if (user.Id >= _nextId) {
                    _nextId = user.Id + 1;
                }
            }
        }

        private void SaveToFile() {
            string usersJson = JsonSerializer.Serialize(_users,
                new JsonSerializerOptions { WriteIndented = true }); // pretty print
            File.WriteAllText(_dataFilePath, usersJson);
        }

        public bool Create(UserDto userDto) {
            try {
                userDto.Id = _nextId++;
                _users.Add(userDto);
                SaveToFile();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public List<UserDto> GetAll() => _users;

        public bool Delete(int id){
            foreach (UserDto user in _users)
            {
                if (user.Id == id) {
                    _users.Remove(user);
                    SaveToFile();
                    return true;
                }
            }
            return false;
        }

        public UserDto GetById(int id) {
            foreach (UserDto user in _users) {
                if (user.Id == id) {
                    return user;
                }
            }
            return null;
        }

        public bool Update(UserDto userDto) {
            var user = GetById(userDto.Id);
            if (user == null) {
                return false;
            }
            if (!string.IsNullOrEmpty(userDto.Password)) {
                user.Password = userDto.Password;
            }
            user.Email = userDto.Email;
            user.Birthday = userDto.Birthday;
            user.Height = userDto.Height;
            user.FirstName = userDto.FirstName;
            SaveToFile();
            return true;
        }

        public UserDto GetByUsername(string username) {
            foreach (UserDto user in _users) {
                if (user.UserName == username) {
                    return user;
                }
            }
            return null;
        }

        public bool Login(string username, string password) {
            UserDto user = GetByUsername(username);
            if (user == null) {
                return false;
            }
            if (user.Password == password) {
                LoggedUserNames.Add(username);
                return true;
            }
            return false;
        }

        public void Logout(string username) {
            if (LoggedUserNames.Contains(username)) {
                LoggedUserNames.Remove(username);
            }
        }

        public void RemoveAvatar(int id) {
            UserDto user = GetById(id);
            if (user != null) {
                user.AvatarFileName = null;
                Update(user);
            }
        }

        public void UpdateAvatar(int id, string avatarFileName) {
            UserDto user = GetById(id);
            if (user != null) {
                user.AvatarFileName = avatarFileName;
                Update(user);
            }
        }
    }
}