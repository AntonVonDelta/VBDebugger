// automatically generated by the FlatBuffers compiler, do not modify


#ifndef FLATBUFFERS_GENERATED_DEBUGGERINFO_NETMODELS_H_
#define FLATBUFFERS_GENERATED_DEBUGGERINFO_NETMODELS_H_

#include "flatbuffers/flatbuffers.h"

// Ensure the included flatbuffers.h is the same version as when this file was
// generated, otherwise it may not be compatible.
static_assert(FLATBUFFERS_VERSION_MAJOR == 23 &&
              FLATBUFFERS_VERSION_MINOR == 5 &&
              FLATBUFFERS_VERSION_REVISION == 26,
             "Non-compatible flatbuffers version included");

namespace NetModels {

struct DebuggerInfo;
struct DebuggerInfoBuilder;
struct DebuggerInfoT;

struct DebuggerInfoT : public ::flatbuffers::NativeTable {
  typedef DebuggerInfo TableType;
  std::string name{};
};

struct DebuggerInfo FLATBUFFERS_FINAL_CLASS : private ::flatbuffers::Table {
  typedef DebuggerInfoT NativeTableType;
  typedef DebuggerInfoBuilder Builder;
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_NAME = 4
  };
  const ::flatbuffers::String *name() const {
    return GetPointer<const ::flatbuffers::String *>(VT_NAME);
  }
  bool Verify(::flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyOffsetRequired(verifier, VT_NAME) &&
           verifier.VerifyString(name()) &&
           verifier.EndTable();
  }
  DebuggerInfoT *UnPack(const ::flatbuffers::resolver_function_t *_resolver = nullptr) const;
  void UnPackTo(DebuggerInfoT *_o, const ::flatbuffers::resolver_function_t *_resolver = nullptr) const;
  static ::flatbuffers::Offset<DebuggerInfo> Pack(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerInfoT* _o, const ::flatbuffers::rehasher_function_t *_rehasher = nullptr);
};

struct DebuggerInfoBuilder {
  typedef DebuggerInfo Table;
  ::flatbuffers::FlatBufferBuilder &fbb_;
  ::flatbuffers::uoffset_t start_;
  void add_name(::flatbuffers::Offset<::flatbuffers::String> name) {
    fbb_.AddOffset(DebuggerInfo::VT_NAME, name);
  }
  explicit DebuggerInfoBuilder(::flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  ::flatbuffers::Offset<DebuggerInfo> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = ::flatbuffers::Offset<DebuggerInfo>(end);
    fbb_.Required(o, DebuggerInfo::VT_NAME);
    return o;
  }
};

inline ::flatbuffers::Offset<DebuggerInfo> CreateDebuggerInfo(
    ::flatbuffers::FlatBufferBuilder &_fbb,
    ::flatbuffers::Offset<::flatbuffers::String> name = 0) {
  DebuggerInfoBuilder builder_(_fbb);
  builder_.add_name(name);
  return builder_.Finish();
}

inline ::flatbuffers::Offset<DebuggerInfo> CreateDebuggerInfoDirect(
    ::flatbuffers::FlatBufferBuilder &_fbb,
    const char *name = nullptr) {
  auto name__ = name ? _fbb.CreateString(name) : 0;
  return NetModels::CreateDebuggerInfo(
      _fbb,
      name__);
}

::flatbuffers::Offset<DebuggerInfo> CreateDebuggerInfo(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerInfoT *_o, const ::flatbuffers::rehasher_function_t *_rehasher = nullptr);

inline DebuggerInfoT *DebuggerInfo::UnPack(const ::flatbuffers::resolver_function_t *_resolver) const {
  auto _o = std::unique_ptr<DebuggerInfoT>(new DebuggerInfoT());
  UnPackTo(_o.get(), _resolver);
  return _o.release();
}

inline void DebuggerInfo::UnPackTo(DebuggerInfoT *_o, const ::flatbuffers::resolver_function_t *_resolver) const {
  (void)_o;
  (void)_resolver;
  { auto _e = name(); if (_e) _o->name = _e->str(); }
}

inline ::flatbuffers::Offset<DebuggerInfo> DebuggerInfo::Pack(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerInfoT* _o, const ::flatbuffers::rehasher_function_t *_rehasher) {
  return CreateDebuggerInfo(_fbb, _o, _rehasher);
}

inline ::flatbuffers::Offset<DebuggerInfo> CreateDebuggerInfo(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerInfoT *_o, const ::flatbuffers::rehasher_function_t *_rehasher) {
  (void)_rehasher;
  (void)_o;
  struct _VectorArgs { ::flatbuffers::FlatBufferBuilder *__fbb; const DebuggerInfoT* __o; const ::flatbuffers::rehasher_function_t *__rehasher; } _va = { &_fbb, _o, _rehasher}; (void)_va;
  auto _name = _fbb.CreateString(_o->name);
  return NetModels::CreateDebuggerInfo(
      _fbb,
      _name);
}

inline const NetModels::DebuggerInfo *GetDebuggerInfo(const void *buf) {
  return ::flatbuffers::GetRoot<NetModels::DebuggerInfo>(buf);
}

inline const NetModels::DebuggerInfo *GetSizePrefixedDebuggerInfo(const void *buf) {
  return ::flatbuffers::GetSizePrefixedRoot<NetModels::DebuggerInfo>(buf);
}

inline const char *DebuggerInfoIdentifier() {
  return "DEBB";
}

inline bool DebuggerInfoBufferHasIdentifier(const void *buf) {
  return ::flatbuffers::BufferHasIdentifier(
      buf, DebuggerInfoIdentifier());
}

inline bool SizePrefixedDebuggerInfoBufferHasIdentifier(const void *buf) {
  return ::flatbuffers::BufferHasIdentifier(
      buf, DebuggerInfoIdentifier(), true);
}

inline bool VerifyDebuggerInfoBuffer(
    ::flatbuffers::Verifier &verifier) {
  return verifier.VerifyBuffer<NetModels::DebuggerInfo>(DebuggerInfoIdentifier());
}

inline bool VerifySizePrefixedDebuggerInfoBuffer(
    ::flatbuffers::Verifier &verifier) {
  return verifier.VerifySizePrefixedBuffer<NetModels::DebuggerInfo>(DebuggerInfoIdentifier());
}

inline void FinishDebuggerInfoBuffer(
    ::flatbuffers::FlatBufferBuilder &fbb,
    ::flatbuffers::Offset<NetModels::DebuggerInfo> root) {
  fbb.Finish(root, DebuggerInfoIdentifier());
}

inline void FinishSizePrefixedDebuggerInfoBuffer(
    ::flatbuffers::FlatBufferBuilder &fbb,
    ::flatbuffers::Offset<NetModels::DebuggerInfo> root) {
  fbb.FinishSizePrefixed(root, DebuggerInfoIdentifier());
}

inline std::unique_ptr<NetModels::DebuggerInfoT> UnPackDebuggerInfo(
    const void *buf,
    const ::flatbuffers::resolver_function_t *res = nullptr) {
  return std::unique_ptr<NetModels::DebuggerInfoT>(GetDebuggerInfo(buf)->UnPack(res));
}

inline std::unique_ptr<NetModels::DebuggerInfoT> UnPackSizePrefixedDebuggerInfo(
    const void *buf,
    const ::flatbuffers::resolver_function_t *res = nullptr) {
  return std::unique_ptr<NetModels::DebuggerInfoT>(GetSizePrefixedDebuggerInfo(buf)->UnPack(res));
}

}  // namespace NetModels

#endif  // FLATBUFFERS_GENERATED_DEBUGGERINFO_NETMODELS_H_
