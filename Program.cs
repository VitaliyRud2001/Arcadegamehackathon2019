using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{

	public class Shoot
	{
		public int X;
		public int Y;
	}

	public static class PlayerInfo
	{
		public static int PlayerPosX = FieldInfo.Length / 2, PlayerPosY = FieldInfo.Height / 2;
		public static char Player = '*';
		private static int _score;

		public static int Score
		{
			get
			{
				return _score;
			}
			set
			{
				if (value <= 0) MainClass.GameOver = true;
				_score = value;
			}
		}
		private static int _health = 5;
		public static int Health
		{
			get
			{
				return _health;

			}
			set
			{
				if(value<=0)
				{
					Score--;
					value = 5;
				}
				_health = value;
			}
		}
		public static void MovePlayer()
		{
			MainClass.SetObject(PlayerPosX, PlayerPosY, Player);	
		}

		public static void DisplayHealthLevel()
		{
			Console.SetCursorPosition(FieldInfo.Length + 10, FieldInfo.Height / 2);
			Console.Write("Health " + Health);
		}

		public static void DisplayResult()
		{
			Console.SetCursorPosition(FieldInfo.Length + 10, FieldInfo.Height / 2+1);
			Console.Write("Your score is " + Score);
		}

	}
	public static class FieldInfo
	{
		public static int Length = 30;
		public static int Height = 25;
	}
	public static class EnemyInfo
	{
		public static int PosX = FieldInfo.Length/2;
		public static int PosY = 1;
		public static char Enemy = '*';
        public static void MoveEnemy()
		{
			MainClass.SetObject(PosX, PosY,Enemy); 
		}
	}

	class MainClass
	{
	
		public static int Ticks = 0;
		public static bool GameOver = false;
		public static void Main(string[] args)
		{
			Console.CursorVisible = false;
			List<Shoot> shoots = new List<Shoot>();
			List<Shoot> EnemyShoot = new List<Shoot>();
			var random = new Random();
			Draw();
			PlayerInfo.DisplayHealthLevel();
			PlayerInfo.DisplayResult();
			while (!GameOver)
			{
				ConsoleKeyInfo pressedKey;
				if (Console.KeyAvailable)
				{
					pressedKey = Console.ReadKey(true);
					SetObject(PlayerInfo.PlayerPosX, PlayerInfo.PlayerPosY, ' ');
					if ((pressedKey.Key == ConsoleKey.UpArrow &&PlayerInfo.PlayerPosY != 1) || (pressedKey.Key == ConsoleKey.DownArrow &&PlayerInfo.PlayerPosY != FieldInfo.Height))
					{
						PlayerInfo.PlayerPosY += (pressedKey.Key == ConsoleKey.DownArrow) ? 1 : -1;
					}
					if ((pressedKey.Key == ConsoleKey.LeftArrow &&PlayerInfo.PlayerPosX != 1) || (pressedKey.Key == ConsoleKey.RightArrow && PlayerInfo.PlayerPosX != FieldInfo.Length))
					{
						PlayerInfo.PlayerPosX += (pressedKey.Key == ConsoleKey.RightArrow) ? 1 : -1;
					}
					if (pressedKey.Key == ConsoleKey.Spacebar)
					{
						Shoot shoot = new Shoot
						{
							X = PlayerInfo.PlayerPosX,
							Y = PlayerInfo.PlayerPosY-1
						};
						shoots.Add(shoot);
					}

					PlayerInfo.MovePlayer();
				}
				foreach (var shoot_ in shoots)
				{
					if (shoot_.Y >= 0)
					{
						if (shoot_.X == EnemyInfo.PosX && shoot_.Y == EnemyInfo.PosY)
						{
							HitEnemy();
							EnemyInfo.PosX = FieldInfo.Length / 2;
						}


						if (shoot_.Y ==0 )
						{
							SetObject(shoot_.X, shoot_.Y, '-');
						}
						else
						{
							SetObject(shoot_.X, shoot_.Y, ' ');
							shoot_.Y--;
							SetObject(shoot_.X, shoot_.Y, '^');
						}
					}
					else
					{
						shoots.Remove(shoot_);
					}
				}

				foreach (var shoot_ in EnemyShoot)
				{
					if (shoot_.Y <= FieldInfo.Height)
					{
						if (shoot_.X == PlayerInfo.PlayerPosX && shoot_.Y == PlayerInfo.PlayerPosY)
						{
							HitPlayer();
						}
						if (shoot_.Y == FieldInfo.Height)
						{
							SetObject(shoot_.X, shoot_.Y, '-');
						}
						else
						{
							SetObject(shoot_.X, shoot_.Y, ' ');
							shoot_.Y++;
							SetObject(shoot_.X, shoot_.Y, 'Y');
						}
					}
					else
					{
						EnemyShoot.Remove(shoot_);
					}
				}
				Ticks++;
				if(Ticks==10)
				{
					SetObject(EnemyInfo.PosX, EnemyInfo.PosY, ' ');
					EnemyInfo.PosX += random.Next(1, 30) > 15 ? 1 : -1;
					EnemyInfo.MoveEnemy();
					Ticks = 0;
				}

				if(random.Next(1,10)==1)
				{
					EnemyShoot.Add(new Shoot
					{
						X = EnemyInfo.PosX,
						Y = EnemyInfo.PosY + 1
					});
				}
					Thread.Sleep(50);	
			}
			Console.Clear();
			DisplayGameOver();
			Console.ReadKey();
		}
		public static void DisplayGameOver()
		{
			Console.WriteLine(@"     _____          __  __ ______         ______      ________ _____  ");
			Console.WriteLine(@"    / ____|   /\   |  \/  |  ____|       / __ \ \    / /  ____|  __ \ ");
			Console.WriteLine(@"   | |  __   /  \  | \  / | |__         | |  | \ \  / /| |__  | |__) |");
			Console.WriteLine(@"   | | |_ | / /\ \ | |\/| |  __|        | |  | |\ \/ / |  __| |  _  / ");
			Console.WriteLine(@"   | |__| |/ ____ \| |  | | |____       | |__| | \  /  | |____| | \ \ ");
			Console.WriteLine(@"    \_____/_/    \_\_|  |_|______|       \____/   \/   |______|_|  \_\");
			Console.WriteLine(@"                                                                      ");

		}


		public static void HitEnemy()
		{
		    PlayerInfo.Score++;
			PlayerInfo.DisplayResult();
		}
		public static void HitPlayer()
		{
			PlayerInfo.Health--;
			PlayerInfo.DisplayHealthLevel();
			PlayerInfo.DisplayResult();
		}
		
		public static void Draw()
		{
			Console.Clear();
			for (int y = 0; y < FieldInfo.Height + 1; ++y)
			{
				for (int x = 0; x < FieldInfo.Length + 1; ++x)
				{
					if (x == 0 || x == FieldInfo.Length) Console.Write('|');
					if (y == 0 || y == FieldInfo.Height) Console.Write('-');
					else
						Console.Write(' ');

				}
				Console.WriteLine();
			}
		}

		public static void SetObject(int PosX, int PosY, char Item)
		{
			Console.SetCursorPosition(PosX, PosY);
			Console.Write(Item);
		}


	}
}