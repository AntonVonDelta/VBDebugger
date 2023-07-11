// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace NetModels
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct DebuggerInfo : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public static DebuggerInfo GetRootAsDebuggerInfo(ByteBuffer _bb) { return GetRootAsDebuggerInfo(_bb, new DebuggerInfo()); }
  public static DebuggerInfo GetRootAsDebuggerInfo(ByteBuffer _bb, DebuggerInfo obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool VerifyDebuggerInfo(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("", false, DebuggerInfoVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DebuggerInfo __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNameBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetNameArray() { return __p.__vector_as_array<byte>(4); }

  public static Offset<NetModels.DebuggerInfo> CreateDebuggerInfo(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset)) {
    builder.StartTable(1);
    DebuggerInfo.AddName(builder, nameOffset);
    return DebuggerInfo.EndDebuggerInfo(builder);
  }

  public static void StartDebuggerInfo(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static Offset<NetModels.DebuggerInfo> EndDebuggerInfo(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 4);  // name
    return new Offset<NetModels.DebuggerInfo>(o);
  }
  public static void FinishDebuggerInfoBuffer(FlatBufferBuilder builder, Offset<NetModels.DebuggerInfo> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedDebuggerInfoBuffer(FlatBufferBuilder builder, Offset<NetModels.DebuggerInfo> offset) { builder.FinishSizePrefixed(offset.Value); }
  public DebuggerInfoT UnPack() {
    var _o = new DebuggerInfoT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(DebuggerInfoT _o) {
    _o.Name = this.Name;
  }
  public static Offset<NetModels.DebuggerInfo> Pack(FlatBufferBuilder builder, DebuggerInfoT _o) {
    if (_o == null) return default(Offset<NetModels.DebuggerInfo>);
    var _name = _o.Name == null ? default(StringOffset) : builder.CreateString(_o.Name);
    return CreateDebuggerInfo(
      builder,
      _name);
  }
}

public class DebuggerInfoT
{
  public string Name { get; set; }

  public DebuggerInfoT() {
    this.Name = null;
  }
  public static DebuggerInfoT DeserializeFromBinary(byte[] fbBuffer) {
    return DebuggerInfo.GetRootAsDebuggerInfo(new ByteBuffer(fbBuffer)).UnPack();
  }
  public byte[] SerializeToBinary() {
    var fbb = new FlatBufferBuilder(0x10000);
    DebuggerInfo.FinishDebuggerInfoBuffer(fbb, DebuggerInfo.Pack(fbb, this));
    return fbb.DataBuffer.ToSizedArray();
  }
}


static public class DebuggerInfoVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyString(tablePos, 4 /*Name*/, true)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}