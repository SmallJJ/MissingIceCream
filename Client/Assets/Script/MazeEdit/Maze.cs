using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Maze
{

    /// 地图上的所有房间
    private Room[,] RoomMatrix;
    //所有通路
    private List<List<Room>> AllRoads;
    // 随机数生成器
    private System.Random random;

    #region public methods
    public Maze(int rows, int cols)
    {
        //实例化随机种子
        random = new System.Random();
        //实例化房间
        this.InitMaze(rows, cols);
        //连接上下 左右的门
        this.JointDoor();
        //固定迷宫四周的门
        this.FixedRoomMatrixOutlineDoor();
        //随机连接出一条通路
        this.JointAllRoomToOneRoad();
    }

    /// <summary>
    /// 获取迷宫转换成的bool数组
    /// </summary>
    /// <returns></returns>
    public bool[,] GetBoolArray()
    {
        return this.RoomToData();
    }

    #endregion

    #region private methods

    /// <summary>
    /// 实例化房间并把通路加到链路
    /// </summary>
    /// <param name="cols"></param>
    /// <param name="rows"></param>
    private void InitMaze(int rows,int cols)
    {
        this.RoomMatrix = new Room[rows,cols];
        this.AllRoads = new List<List<Room>>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
				//开始每一个房间作为一条路
                List<Room> road = new List<Room>();
                this.RoomMatrix[row,col] = new Room();
                road.Add(this.RoomMatrix[row, col]);
				//添加到所有通路
                this.AllRoads.Add(road);
            }
        }
    }

    /// <summary>
    /// 连接上下门和左右门
    /// </summary>
    private void JointDoor()
    {
        //每个房间的下门和下面房间的上门公用一个门 
        for (int row = 0; row < this.RoomMatrix.GetLength(0)-1; row++)   
        {
            for (int col = 0; col < this.RoomMatrix.GetLength(1); col++)  
            {
                this.RoomMatrix[row, col].BottomDoor = this.RoomMatrix[row+1, col].TopDoor;   
            }
        }
        //每个房间的右门和右边房间的左门公用一个门 
        for (int row = 0; row < this.RoomMatrix.GetLength(0); row++)
        {
            for (int col = 0; col < this.RoomMatrix.GetLength(1) - 1; col++)
            {
                this.RoomMatrix[row, col].RightDoor = this.RoomMatrix[row, col + 1].LeftDoor;
            }
        }
    }

    /// <summary>
    /// 固定四周的的门
    /// </summary>
    private void FixedRoomMatrixOutlineDoor()
    {
        //关闭第一行上面的门
        for (int i = 0; i < this.RoomMatrix.GetLength(1); i++)
        {
            this.RoomMatrix[0, i].TopDoor.SetFixed();
        }

        //关闭最后一行下面的门
        for (int i = 0; i < this.RoomMatrix.GetLength(1); i++)
        {
            this.RoomMatrix[this.RoomMatrix.GetLength(0) - 1,i].BottomDoor.SetFixed();
        }

        //关闭第一列左边的门
        for (int i = 0; i < this.RoomMatrix.GetLength(0); i++)
        {
            this.RoomMatrix[i, 0].LeftDoor.SetFixed();
        }

        //关闭最后一列右边的门
        for (int i = 0; i <this.RoomMatrix.GetLength(0) ; i++)
        {
            this.RoomMatrix[i,this.RoomMatrix.GetLength(1) - 1].RightDoor.SetFixed();
        }
    }

    private void JointAllRoomToOneRoad()
    {
        int randomTemp;
        while(!this.AllRoomJointed())
        {
            //随机从链路上获取一条通路房间
            randomTemp=this.random.Next();
            List<Room> rooms = this.AllRoads[randomTemp % this.AllRoads.Count];
            //获取通路房间轮廓上的门
            List<Door> roadsOutLineDoors = this.GetOutLineDoors(rooms);
            //从通路轮廓上的门随机取一扇门准备打开
            randomTemp = this.random.Next();
            Door doorReadToOpen = roadsOutLineDoors[randomTemp % roadsOutLineDoors.Count];
            //找到公用这扇门的两个条通路(每条通路可能是一个房间,也可能是一串房间)
            List<List<Room>> roadsUseTheDoor = this.GetRoadsUseTheDoor(doorReadToOpen);
            //合并两条通路,得到新的通路
            List<Room> newRoad = this.MergeTwoRoads(roadsUseTheDoor);
            //从全部通路移除共用一个门的路
            this.RemoveTwoRoads(roadsUseTheDoor);
            //添加合并后的通路到全部通路中
            this.AllRoads.Add(newRoad);
            //将准备打开的门打开
            doorReadToOpen.OpenTheDoor();
        }
    }


    //移除共用一个门的路
    private void RemoveTwoRoads( List<List<Room>> roads)
    {
        foreach ( List<Room> road  in  roads)
        {
            this.AllRoads.Remove(road);
        }
    }

    // 合并两条通路
    private List<Room> MergeTwoRoads(List<List<Room>> roads)
    {
        List<Room> newRoad = new List<Room>();
        foreach (List<Room>listRoom in roads)
        {
            foreach (Room room in listRoom)
            {
                newRoad.Add(room);
            }
        }
        return newRoad;
    }

    //比两扇门是不是同一个门
    private bool EqualsTwoDoors(Door doorSource,Door doorTarget)
    {
        if (!doorSource.IsFixed && doorSource.IsLocked)
        {
            if (doorSource.Equals(doorTarget))
                return true;
        }
        return false;
    }

    //获取共用一扇门的两条通路
    private List<List<Room>> GetRoadsUseTheDoor(Door door)
    {
        List<List<Room>> roads =new  List<List<Room>>();
        foreach (List<Room> listRoom in AllRoads)
        {
            foreach (Room room in listRoom)
            {
                if (this.EqualsTwoDoors(room.LeftDoor, door)
                    || this.EqualsTwoDoors(room.RightDoor, door)
                    || this.EqualsTwoDoors(room.TopDoor, door)
                    || this.EqualsTwoDoors(room.BottomDoor, door))
                {
                    roads.Add(listRoom);
                    break;
                }
            }
        }
        return roads;
    }

    //获取轮廓上的门
    private List<Door> GetOutLineDoors(List<Room> rooms)
    {
        List<Door> outLineDoors=new List<Door> ();
        foreach (Room room in rooms)
        {
            this.AddPassageOutLineDoor(room.LeftDoor, outLineDoors);
            this.AddPassageOutLineDoor(room.RightDoor,outLineDoors);
            this.AddPassageOutLineDoor(room.TopDoor,outLineDoors);
            this.AddPassageOutLineDoor(room.BottomDoor,outLineDoors);
        }
        return outLineDoors;
    }

    //添加一条通路的轮廓上的门
    private void AddPassageOutLineDoor(Door door,List<Door> outLineDoors)
    {
        if(door.IsLocked&&!door.IsFixed)
        {
			if(!outLineDoors.Contains(door))
			{
            	outLineDoors.Add(door);
			}
			else
			{
				outLineDoors.Remove(door);
			}
        }
    }

    private bool AllRoomJointed()
    {
        if (this.AllRoads.Count == 1)
            return true;
        else
            return false;
    }

    private bool[,] RoomToData()
    {
        //一个房间=9个块,房间之间公用左右上面的门,所以迷宫最后的长和宽为*2+1
        //中间的块是房间,是一直开着的
        bool[,] dataMatrix=new bool[this.RoomMatrix.GetLength(0)*2+1,this.RoomMatrix.GetLength(1)*2+1];
        //将是房间的先填充为真
        this.SetOtherDoorFill(dataMatrix);
        // 遍历每个房间， 并将该房间的四个方向的门的开关状态映射到保存迷宫的数组中
        for (int row = 0; row < this.RoomMatrix.GetLength(0); row++)
        {
            for (int col = 0; col < this.RoomMatrix.GetLength(1); col++)
            {
                this.SetData(dataMatrix,row,col,0,-1,this.RoomMatrix[row,col].LeftDoor.IsLocked);
                this.SetData(dataMatrix, row, col, 0,1, this.RoomMatrix[row, col].RightDoor.IsLocked);
                this.SetData(dataMatrix, row, col, -1, 0, this.RoomMatrix[row, col].TopDoor.IsLocked);      //因为2维数组,是行列的矩阵,从左上角为0,0坐标,所以Y坐标向上为-1,向下为+1
                this.SetData(dataMatrix, row, col, 1, 0, this.RoomMatrix[row, col].BottomDoor.IsLocked);
            }
        }
        return dataMatrix;
    }

    private void SetData(bool[,] dataMatrix,int yPos,int xPos,int yOffset,int xOffset,bool isClose)
    {
        int y=yPos * 2 + 1 + yOffset;
        int x=xPos * 2 +1+ xOffset;
        //这是左 右  上  下的门 获取房间左右上下门的状态
        dataMatrix[y,x ] = isClose;
    }

    /// 将x与y方向的第奇数格预先填充为真
    private void SetOtherDoorFill(bool[,] dataMatrix)
    {
        //因为房间是连着的,有公用门,行列+=2就是左上 右上 左下 右下 的门
        for (int i = 0; i < dataMatrix.GetLength(0); i+=2)
        {
            for (int j = 0; j < dataMatrix.GetLength(1); j+=2)
            {
                dataMatrix[i, j] = true;
            }
        }
    }
    #endregion
}

public class Room
{
    /// <summary>
    /// 四个方向的门
    /// </summary>
    public Door TopDoor { get; set; }
    public Door RightDoor { get; set; }
    public Door BottomDoor { get; set; }
    public Door LeftDoor { get; set; }

    public Room()
    {
        this.TopDoor = new Door();
        this.RightDoor = new Door();
        this.BottomDoor = new Door();
        this.LeftDoor = new Door();
    }
}

public class Door
{
    public Door()
    {
        this.IsLocked = true;
    }
    //是否是固定的
    public bool IsFixed { get; set; }
    //是否是上锁的
    public bool IsLocked { get; set; }

    public void CloseTheDoor()
    {
        this.IsLocked=true;
    }

    public void OpenTheDoor()
    {
        this.IsLocked=false;
    }

    public void SetFixed()
    {
        this.IsFixed = true;
    }
}
