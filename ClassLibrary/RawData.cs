using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class RawData
    {
        public double leftLimitOfSegment;
        public double rightLimitOfSegment;
        public int numberOfNodes;
        public bool isUniform;
        public FRaw function;
        public double[] nodesOfGrid;
        public double[] valuesInNodes;

        public RawData(double leftLimitOfSegment, double rightLimitOfSegment, int numberOfNodes, bool isUniform, FRaw function)
        {
            this.leftLimitOfSegment = leftLimitOfSegment;
            this.rightLimitOfSegment = rightLimitOfSegment;
            this.numberOfNodes = numberOfNodes;
            this.isUniform = isUniform;
            this.function = function;
            nodesOfGrid = new double[numberOfNodes];

            nodesOfGrid[0] = leftLimitOfSegment;
            nodesOfGrid[numberOfNodes - 1] = rightLimitOfSegment;
            if (isUniform)
            {
                var stepOfTheGrid = (rightLimitOfSegment - leftLimitOfSegment) / (numberOfNodes - 1);
                for (int i = 1; i < numberOfNodes - 1; i++)
                {
                    nodesOfGrid[i] = leftLimitOfSegment + stepOfTheGrid * i;
                }
            }
            else
            {
                var rnd = new Random();
                for (int i = 1; i < numberOfNodes - 1; i++)
                {
                    nodesOfGrid[i] = rightLimitOfSegment - rnd.NextDouble() * (rightLimitOfSegment - leftLimitOfSegment);
                }
            }
            valuesInNodes = new double[numberOfNodes];
        }
        
        public RawData()
        {
            //this.function = function;
            nodesOfGrid = new double[numberOfNodes];
            valuesInNodes = new double[numberOfNodes];
        }

        public RawData(string fileName)
        {
            var obj = new RawData();
            Load(fileName, ref obj);
            this.leftLimitOfSegment = obj.leftLimitOfSegment;
            this.rightLimitOfSegment = obj.rightLimitOfSegment;
            this.numberOfNodes = obj.numberOfNodes;
            this.isUniform = obj.isUniform;
            this.function = cube;
            this.nodesOfGrid = new double[obj.numberOfNodes];
            this.valuesInNodes = new double[obj.numberOfNodes];
            for (int i = 0; i < obj.numberOfNodes; i++)
            {
                this.nodesOfGrid[i] = obj.nodesOfGrid[i];
                this.valuesInNodes[i] = obj.valuesInNodes[i];
            }
        }

        public void initializeTheValues()
        {
            for(int i = 0; i < numberOfNodes; i++)
            {
                valuesInNodes[i] = function(nodesOfGrid[i]); 
            }
        }

        public bool Save(string filename)
        {
            var file = new FileStream(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            try
            {
                writer.Write(leftLimitOfSegment);
                writer.Write(rightLimitOfSegment);
                writer.Write(numberOfNodes);
                writer.Write(isUniform);
                //writer.Write(function.ToString());
                for(int i = 0; i < numberOfNodes; i++)
                {
                    writer.Write(nodesOfGrid[i]);
                    writer.Write(valuesInNodes[i]);
                }
                return true;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: THE STREAM ERROR");
                return false;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ERROR: STRING IS NULL");
                return false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("ERROR: THE STREAM HAS BEEN CLOSED");
                return false;
            }
            catch
            {
                Console.WriteLine("ERROR: UNEXPECTED ERROR WITH WRITING IN FILE");
                return false;
            }
            finally
            {
                writer.Dispose();
                file.Dispose();
            }
        }

        static public void Load(string filename, ref RawData rawData)
        {
            FileStream file = null;
            BinaryReader reader = null;
            try
            {
                file = new FileStream(filename, FileMode.Open);
                reader = new BinaryReader(file);
                rawData.leftLimitOfSegment = reader.ReadDouble();
                rawData.rightLimitOfSegment = reader.ReadDouble();
                rawData.numberOfNodes = reader.ReadInt32();
                rawData.isUniform = reader.ReadBoolean();
                //rawData.function = (FRaw)reader.ReadString();
                FRaw func = cube;
                rawData.function = func;
                rawData.nodesOfGrid = new double[rawData.numberOfNodes];
                rawData.valuesInNodes = new double[rawData.numberOfNodes];
                for (int i = 0; i < rawData.numberOfNodes; i++)
                {
                    rawData.nodesOfGrid[i] = reader.ReadDouble();
                    rawData.valuesInNodes[i] = reader.ReadDouble();
                }
                //return true;
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("ERROR: THE STREAM IS ENDED");
               //return false;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("ERROR: THE NAME OF THE FILE IS INVALID");
                //return false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("ERROR: THE STREAM HAS BEEN CLOSED");
               //return false;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: THE STREAM ERROR");
               //return false;
            }
            catch
            {
                Console.WriteLine("ERROR: UNEXPECTED ERROR WITH READING FROM FILE");
                //return false;
            }
            finally
            {
                if (file != null)
                {
                    file.Dispose();
                }
                if (reader != null)
                    reader.Dispose();
            }
        }
        public string ToLongString(string format)
        {
            string res = $"Left limit of range is {string.Format(format, leftLimitOfSegment)}, " + "\n" +
                $"Right limit of range is {string.Format(format, rightLimitOfSegment)} " + "\n" +
                $"Number of nodes is  {numberOfNodes}" + "\n" +
                $"Parameter of uniform is {isUniform}" + "\n" +
                $"Function is {function.Method}" + "\n" +
                "    Coordinates and values: " + "\n";
            for (int i = 0; i < this.numberOfNodes; i++)
            {
                res = res + $"    {i}:" + $"{string.Format(format, this.nodesOfGrid[i])}, " + $"{string.Format(format, this.valuesInNodes[i])}" + "\n";
            }
            return res;
        }

        static double cube(double x)
        {
            return x * x * x;
        }
    }
}
