// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace NetModels
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct DebugCommand : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public static DebugCommand GetRootAsDebugCommand(ByteBuffer _bb) { return GetRootAsDebugCommand(_bb, new DebugCommand()); }
  public static DebugCommand GetRootAsDebugCommand(ByteBuffer _bb, DebugCommand obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool DebugCommandBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "DEBB"); }
  public static bool VerifyDebugCommand(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("DEBB", false, DebugCommandVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DebugCommand __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public NetModels.CommandType CommandType { get { int o = __p.__offset(4); return o != 0 ? (NetModels.CommandType)__p.bb.GetInt(o + __p.bb_pos) : NetModels.CommandType.Pause; } }

  public static Offset<NetModels.DebugCommand> CreateDebugCommand(FlatBufferBuilder builder,
      NetModels.CommandType command_type = NetModels.CommandType.Pause) {
    builder.StartTable(1);
    DebugCommand.AddCommandType(builder, command_type);
    return DebugCommand.EndDebugCommand(builder);
  }

  public static void StartDebugCommand(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddCommandType(FlatBufferBuilder builder, NetModels.CommandType commandType) { builder.AddInt(0, (int)commandType, 0); }
  public static Offset<NetModels.DebugCommand> EndDebugCommand(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<NetModels.DebugCommand>(o);
  }
  public static void FinishDebugCommandBuffer(FlatBufferBuilder builder, Offset<NetModels.DebugCommand> offset) { builder.Finish(offset.Value, "DEBB"); }
  public static void FinishSizePrefixedDebugCommandBuffer(FlatBufferBuilder builder, Offset<NetModels.DebugCommand> offset) { builder.FinishSizePrefixed(offset.Value, "DEBB"); }
  public DebugCommandT UnPack() {
    var _o = new DebugCommandT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(DebugCommandT _o) {
    _o.CommandType = this.CommandType;
  }
  public static Offset<NetModels.DebugCommand> Pack(FlatBufferBuilder builder, DebugCommandT _o) {
    if (_o == null) return default(Offset<NetModels.DebugCommand>);
    return CreateDebugCommand(
      builder,
      _o.CommandType);
  }
}

public class DebugCommandT
{
  public NetModels.CommandType CommandType { get; set; }

  public DebugCommandT() {
    this.CommandType = NetModels.CommandType.Pause;
  }
  public static DebugCommandT DeserializeFromBinary(byte[] fbBuffer) {
    return DebugCommand.GetRootAsDebugCommand(new ByteBuffer(fbBuffer)).UnPack();
  }
  public byte[] SerializeToBinary() {
    var fbb = new FlatBufferBuilder(0x10000);
    DebugCommand.FinishDebugCommandBuffer(fbb, DebugCommand.Pack(fbb, this));
    return fbb.DataBuffer.ToSizedArray();
  }
}


static public class DebugCommandVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*CommandType*/, 4 /*NetModels.CommandType*/, 4, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}