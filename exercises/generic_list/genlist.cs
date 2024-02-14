public class genlist<T>{

public T[] data;
public T this[int i] {get{return data[i];} set{data[i]=value;}}
public int size => data.Length;

public genlist(){data = new T[0];}
public void add (T item){
	T[] newdata = new T[data.Length+1];
	for(int i=0;i < data.Length;i++)newdata[i]=data[i];
	newdata[data.Length] = item;
	data = newdata;
	}

}
