export function lookupDropDown(lookups) {
  return lookups.map(lookup => {
    return {
      value: lookup.id,
      text: lookup.id + " " + lookup.name
    };
  });
}
